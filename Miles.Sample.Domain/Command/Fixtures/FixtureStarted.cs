﻿using System;

namespace Miles.Sample.Domain.Command.Fixtures
{
    class FixtureStarted
    {
        public Guid Id { get; set; }

        public DateTime When { get; set; }
    }
}