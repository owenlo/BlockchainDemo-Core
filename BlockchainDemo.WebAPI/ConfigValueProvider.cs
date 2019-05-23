using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BlockchainDemo.WebAPI
{
    //Source: https://stackoverflow.com/a/53394150
    public static class ConfigValueProvider
    {
        private static readonly IConfigurationRoot Configuration;

        static ConfigValueProvider()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public static string Get(string name)
        {
            return Configuration[name];
        }
    }
}
