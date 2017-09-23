﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportCommunityRM.WebSite.ViewModels.Team
{
    public class DetailViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Min. Birth Year")]
        public int? MinBirthYear { get; set; }

        [Display(Name = "Max. Birth Year")]
        public int? MaxBirthYear { get; set; }

        public IEnumerable<Player> Players { get; set; }

        public IEnumerable<Coach> Coaches { get; set; }

        public Activity[] Activities { get; set; }

        public class Player
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public DateTime BirthDate { get; set; }
        }

        public class Coach
        {
            public Guid Id { get; set; }

            public string Name { get; set; }
        }

        public class Activity
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public ActivityType ActivityType { get; set; }
        }

        public enum ActivityType
        {
            Match,
            Tournament,
            Training
        }
    }
}