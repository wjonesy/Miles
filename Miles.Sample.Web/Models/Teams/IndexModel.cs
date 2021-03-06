﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Miles.Sample.Web.Models.Leagues
{
    public class IndexModel
    {
        public List<IndexModelLeague> Leagues { get; set; }
    }

    public class IndexModelLeague
    {
        public string Abbreviation { get; set; }

        public string Name { get; set; }
    }
}