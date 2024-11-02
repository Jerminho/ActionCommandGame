using ActionCommandGame.Configuration;
using ActionCommandGame.DTO.Results; 
using ActionCommandGame.Extensions;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class GameService : IGameService
    {
        private readonly AppSettings _appSettings;
        private readonly ActionCommandGameDbContext _database;
        private readonly IPlayerService _playerService;
        private readonly IPositiveGameEventService _positiveGameEventService;
        private readonly INegativeGameEventService _negativeGameEventService;
        private readonly IItemService _itemService;
        private readonly IPlayerItemService _playerItemService;

        public GameService(
            AppSettings appSettings,
            ActionCommandGameDbContext database,
            IPlayerService playerService,
            IPositiveGameEventService positiveGameEventService,
            INegativeGameEventService negativeGameEventService,
            IItemService itemService,
            IPlayerItemService playerItemService)
        {
            _appSettings = appSettings;
            _database = database;
            _playerService = playerService;
            _positiveGameEventService = positiveGameEventService;
            _negativeGameEventService = negativeGameEventService;
            _itemService = itemService;
            _playerItemService = playerItemService;
        }

        public async Task<ServiceResult<GameResultDto>> PerformAction(int playerId)
        {
            // Check Cooldown
            var player = await _database.Players
                .Include(p => p.CurrentFuelPlayerItem).ThenInclude(pi => pi!.Item)
                .Include(p => p.CurrentAttackPlayerItem).ThenInclude(pi => pi!.Item)
                .Include(p => p.CurrentDefensePlayerItem).ThenInclude(pi => pi!.Item)
                .SingleOrDefaultAsync(p => p.Id == playerId);

            ServiceResult<PlayerResultDto> playerResult;

            if (player is null)
            {
                return new ServiceResult<GameResultDto>
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage { Code = "NotFound", Message = $"{nameof(Player)} was not found" }
                    }
                };
            }

            if (player.LastActionExecutedDateTime.HasValue)
            {
                var elapsedSeconds = DateTime.UtcNow.Subtract(player.LastActionExecutedDateTime.Value).TotalSeconds;
                var cooldownSeconds = _appSettings.DefaultCooldownSeconds;

                if (player.CurrentFuelPlayerItem is not null)
                {
                    cooldownSeconds = player.CurrentFuelPlayerItem.Item.ActionCooldownSeconds;
                }

                if (elapsedSeconds < cooldownSeconds)
                {
                    var waitSeconds = Math.Ceiling(cooldownSeconds - elapsedSeconds);
                    var waitText = $"You are still a bit tired. You have to wait another {waitSeconds} seconds.";
                    playerResult = await _playerService.Get(playerId);
                    return new ServiceResult<GameResultDto>
                    {
                        Data = new GameResultDto { Player = playerResult.Data },
                        Messages = new List<ServiceMessage> { new ServiceMessage { Code = "Cooldown", Message = waitText } }
                    };
                }
            }

            player.LastActionExecutedDateTime = DateTime.UtcNow;

            var hasAttackItem = player.CurrentAttackPlayerItemId.HasValue;

            // Retrieve positive game event
            var positiveGameEventResult = await _positiveGameEventService.GetRandomPositiveGameEvent(hasAttackItem);
            if (positiveGameEventResult.Data is null)
            {
                return new ServiceResult<GameResultDto>
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "Error",
                            Message = "Something went wrong getting the Positive Game Event.",
                            MessagePriority = MessagePriority.Error
                        }
                    }
                };
            }

            // Retrieve negative game event
            var negativeGameEventResult = await _negativeGameEventService.GetRandomNegativeGameEvent();
            if (negativeGameEventResult.Data is null)
            {
                return new ServiceResult<GameResultDto>
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "Error",
                            Message = "Something went wrong getting the Negative Game Event.",
                            MessagePriority = MessagePriority.Error
                        }
                    }
                };
            }

            var oldLevel = player.GetLevel();

            player.Money += positiveGameEventResult.Data.Money;
            player.Experience += positiveGameEventResult.Data.Experience;

            var newLevel = player.GetLevel();
            var levelMessages = new List<ServiceMessage>();

            // Check if we leveled up
            if (oldLevel < newLevel)
            {
                levelMessages = new List<ServiceMessage>
                {
                    new ServiceMessage { Code = "LevelUp", Message = $"Congratulations, you arrived at level {newLevel}" }
                };
            }

            // Consume fuel
            var fuelMessages = await ConsumeFuel(player);

            var attackMessages = new List<ServiceMessage>();
            // Consume attack when we got some loot
            if (positiveGameEventResult.Data.Money > 0)
            {
                var consumeAttackMessages = await ConsumeAttack(player);
                attackMessages.AddRange(consumeAttackMessages);
            }

            var defenseMessages = new List<ServiceMessage>();
            var negativeGameEventMessages = new List<ServiceMessage>();
            var negativeGameEvent = negativeGameEventResult.Data; // Get the negative game event here

            if (negativeGameEvent != null)
            {
                // Check defense consumption
                if (player.CurrentDefensePlayerItem != null)
                {
                    negativeGameEventMessages.Add(new ServiceMessage { Code = "DefenseWithGear", Message = negativeGameEvent.DefenseWithGearDescription });
                    var consumeDefenseMessages = await ConsumeDefense(player, negativeGameEvent.DefenseLoss);
                    defenseMessages.AddRange(consumeDefenseMessages);
                }
                else
                {
                    negativeGameEventMessages.Add(new ServiceMessage { Code = "DefenseWithoutGear", Message = negativeGameEvent.DefenseWithoutGearDescription });

                    // If we have no defense item, consume the defense loss from Fuel and Attack
                    var consumeFuelMessages = await ConsumeFuel(player, negativeGameEvent.DefenseLoss);
                    var consumeAttackMessages = await ConsumeAttack(player);
                    defenseMessages.AddRange(consumeFuelMessages);
                    defenseMessages.AddRange(consumeAttackMessages);
                }
            }

            var warningMessages = GetWarningMessages(player);

            // Save Player
            await _database.SaveChangesAsync();

            playerResult = await _playerService.Get(playerId);
            var gameResult = new GameResultDto
            {
                Player = playerResult.Data,
                PositiveGameEvent = positiveGameEventResult.Data,
                NegativeGameEvent = negativeGameEvent,
                NegativeGameEventMessages = (IList<DTO.ServiceMessage>)negativeGameEventMessages
            };

            var serviceResult = new ServiceResult<GameResultDto>
            {
                Data = gameResult
            };

            // Add all the messages to the player
            serviceResult.WithMessages(levelMessages);
            serviceResult.WithMessages(warningMessages);
            serviceResult.WithMessages(fuelMessages);
            serviceResult.WithMessages(attackMessages);
            serviceResult.WithMessages(defenseMessages);

            return serviceResult;
        }

        public async Task<ServiceResult<BuyResultDto>> Buy(int playerId, int itemId)
        {
            var player = await _database.Players.SingleOrDefaultAsync(p => p.Id == playerId);
            if (player == null)
            {
                return new ServiceResult<BuyResultDto>().PlayerNotFound();
            }

            var item = await _database.Items.SingleOrDefaultAsync(i => i.Id == itemId);
            if (item == null)
            {
                return new ServiceResult<BuyResultDto>().ItemNotFound();
            }

            if (item.Price > player.Money)
            {
                return new ServiceResult<BuyResultDto>().NotEnoughMoney();
            }

            await _playerItemService.Create(playerId, itemId);
            player.Money -= item.Price;

            // Save Changes
            await _database.SaveChangesAsync();

            // Get the result objects using DTOs
            var playerResult = await _playerService.Get(playerId);
            var itemResult = await _itemService.Get(itemId);

            var buyResult = new BuyResultDto
            {
                Player = playerResult.Data,
                Item = itemResult.Data
            };
            return new ServiceResult<BuyResultDto> { Data = buyResult };
        }

        private async Task<IList<ServiceMessage>> ConsumeFuel(Player player, int fuelLoss = 1)
        {
            if (player.CurrentFuelPlayerItem != null && player.CurrentFuelPlayerItemId.HasValue)
            {
                player.CurrentFuelPlayerItem.RemainingFuel -= fuelLoss;
                if (player.CurrentFuelPlayerItem.RemainingFuel <= 0)
                {
                    await _playerItemService.Delete(player.CurrentFuelPlayerItemId.Value);

                    // Load a new Fuel Item from inventory
                    var newFuelItem = await _database.PlayerItems
                        .Include(pi => pi.Item)
                        .Where(pi => pi.PlayerId == player.Id && pi.Item.Fuel > 0)
                        .OrderByDescending(pi => pi.Item.Fuel)
                        .FirstOrDefaultAsync();

                    if (newFuelItem != null)
                    {
                        player.CurrentFuelPlayerItem = newFuelItem;
                        player.CurrentFuelPlayerItemId = newFuelItem.Id;
                        return new List<ServiceMessage>
                        {
                            new ServiceMessage
                            {
                                Code = "ReloadedFuel",
                                Message = $"You are hungry and open a new {newFuelItem.Item.Name} to fill your belly."
                            }
                        };
                    }
                    player.CurrentFuelPlayerItem = null;
                    player.CurrentFuelPlayerItemId = null;
                    return new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "NoFuel",
                            Message = $"You have run out of fuel. You cannot take further action."
                        }
                    };
                }
            }
            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> ConsumeAttack(Player player)
        {
            if (player.CurrentAttackPlayerItem != null)
            {
                player.CurrentAttackPlayerItem.RemainingAttack--;
                if (player.CurrentAttackPlayerItem.RemainingAttack <= 0)
                {
                    await _playerItemService.Delete(player.CurrentAttackPlayerItemId.Value);
                    player.CurrentAttackPlayerItem = null;
                    player.CurrentAttackPlayerItemId = null;
                    return new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "NoAttackItem",
                            Message = $"You have run out of your attack item."
                        }
                    };
                }
            }
            return new List<ServiceMessage>();
        }

        private async Task<IList<ServiceMessage>> ConsumeDefense(Player player, int defenseLoss)
        {
            if (player.CurrentDefensePlayerItem != null)
            {
                player.CurrentDefensePlayerItem.RemainingDefense -= defenseLoss;
                if (player.CurrentDefensePlayerItem.RemainingDefense <= 0)
                {
                    await _playerItemService.Delete(player.CurrentDefensePlayerItemId.Value);
                    player.CurrentDefensePlayerItem = null;
                    player.CurrentDefensePlayerItemId = null;
                    return new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "NoDefenseItem",
                            Message = $"You have run out of your defense item."
                        }
                    };
                }
            }
            return new List<ServiceMessage>();
        }

        private List<ServiceMessage> GetWarningMessages(Player player)
        {
            var messages = new List<ServiceMessage>();
            if (player.CurrentFuelPlayerItem?.RemainingFuel <= 0)
            {
                messages.Add(new ServiceMessage
                {
                    Code = "Warning",
                    Message = "You are out of fuel. Please acquire new fuel to continue."
                });
            }
            if (player.CurrentAttackPlayerItem?.RemainingAttack <= 0)
            {
                messages.Add(new ServiceMessage
                {
                    Code = "Warning",
                    Message = "You are out of attack items. Please acquire new attack items."
                });
            }
            if (player.CurrentDefensePlayerItem?.RemainingDefense <= 0)
            {
                messages.Add(new ServiceMessage
                {
                    Code = "Warning",
                    Message = "You are out of defense items. Please acquire new defense items."
                });
            }
            return messages;
        }
    }
}
