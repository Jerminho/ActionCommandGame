﻿using ActionCommandGame.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.DTO.Results
{
    public class PlayerResultDto:IHasExperience
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public DateTime? LastActionExecutedDateTime { get; set; }
        public int? CurrentFuelId { get; set; }
        public string? CurrentFuelName { get; set; }
        public int CurrentFuelActionCooldownSeconds { get; set; }
        public int TotalFuel { get; set; }
        public int RemainingFuel { get; set; }
        public int? CurrentAttackId { get; set; }
        public string? CurrentAttackName { get; set; }
        public int TotalAttack { get; set; }
        public int RemainingAttack { get; set; }
        public int? CurrentDefenseId { get; set; }
        public string? CurrentDefenseName { get; set; }
        public int TotalDefense { get; set; }
        public int RemainingDefense { get; set; }
        public int NumberOfInventoryItems { get; set; }
    }
}
