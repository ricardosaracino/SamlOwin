﻿using System.Reflection;
using AutoMapper;
using AutoMapper.Configuration;

namespace SamlOwin.Providers
{
    public static class AutoMapperProvider
    {
        private static MapperConfiguration _mapperConfiguration;

        public static Mapper GetMapper()
        {
            if (_mapperConfiguration == null)
            {
                _mapperConfiguration = CreateConfiguration();
                
            }

            return new Mapper(_mapperConfiguration);
        }

        private static MapperConfiguration CreateConfiguration()
        {
            var cfg = new MapperConfigurationExpression {AllowNullCollections = true};


            cfg.AddMaps(Assembly.GetExecutingAssembly());

            return new MapperConfiguration(cfg);
        }
    }
}