﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.DTO.Results
{
    public class PlayerItemResultDto
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public required string PlayerName { get; set; }
        public int ItemId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Fuel { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int ActionCooldownSeconds { get; set; }
        public int RemainingFuel { get; set; }
        public int RemainingAttack { get; set; }
        public int RemainingDefense { get; set; }
    }
}
