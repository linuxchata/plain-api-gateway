﻿using System.Collections.Generic;

namespace PlainApiGateway.Configuration
{
    public sealed class PlainConfiguration
    {
        public List<PlainRouteConfiguration> Routes { get; set; }

        public ushort? TimeoutInSeconds { get; set; }
    }
}
