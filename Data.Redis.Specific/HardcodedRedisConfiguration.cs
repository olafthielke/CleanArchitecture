﻿using Data.Redis.Common.Interfaces;

namespace Data.Redis.Specific
{
    public class HardcodedRedisConfiguration : IRedisConfiguration
    {
        // Imagine that Host is being read from a config file.
        public string Host => "localhost";
    }
}
