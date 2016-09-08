using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Miles.Sample.Web.Models.Leagues
{
    public class RegisterTeamModel
    {
        public List<string> Teams { get; set; }

        public string LeagueId { get; set; }

        [Required]
        public string Team { get; set; }
    }
}