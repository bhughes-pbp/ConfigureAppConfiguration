using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ConfigureAppConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var host = new HostBuilder()
                .ConfigureHostConfiguration(config =>
                {
                    config.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    var hostConfig = hostContext.Configuration; // MARK A
                    
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    var hostConfig = hostContext.Configuration; // MARK B
                    // we expect Providers configured in the previous
                    // ConfigureAppConfiguration to be listed
                    // in providers.Sources, so that we can load values using
                    // those providers
                    var loaded = hostConfig.GetValue<string>("SampleSetting"); // loaded == null
                    
                    // we can get the value after explicitly Build()ing though,
                    // but we could also have done this at the end of the previous
                    // ConfigureAppConfiguration, so the value of separating
                    // config-loading into multiple ConfigureAppConfiguration
                    // isn't clear
                    loaded = config.Build().GetValue<string>("SampleSetting"); // loaded == "LoadMe"
                })
                .Build();
        }
    }
}
