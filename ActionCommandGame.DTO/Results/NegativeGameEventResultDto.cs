﻿using ActionCommandGame.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.DTO.Results
{
    public class NegativeGameEventResultDto : IHasProbability
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string DefenseWithGearDescription { get; set; }
        public required string DefenseWithoutGearDescription { get; set; }
        public int DefenseLoss { get; set; }
        public int Probability { get; set; }
    }
}