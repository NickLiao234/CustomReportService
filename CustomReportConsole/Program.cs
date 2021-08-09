using CustomReport.Library;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomReportConsole
{
    /// <summary>
    /// 啟動類別
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 主程式
        /// </summary>
        /// <param name="args">args</param>
        /// <returns></returns>
        public static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<App>();
            serviceCollection.AddSingleton<CustomServiceHandler>(c =>
            {
                var services = new List<ICustomService>();
                services.Add(new MockCustomReportService(100, 500));
                services.Add(new MockCustomReportService(100, 1000));
                services.Add(new MockCustomReportService(100, 5000));
                services.Add(new MockCustomReportService(100, 5000));
                services.Add(new MockCustomReportService(100, 10000));
                return new CustomServiceHandler(services);
            });
            serviceCollection.AddSingleton<CustomServiceWithMaxRequestCountHandler>(c =>
            {
                var services = new List<ICustomService>();
                services.Add(new MockCustomReportService(10, 500));
                services.Add(new MockCustomReportService(10, 1000));
                services.Add(new MockCustomReportService(10, 5000));
                services.Add(new MockCustomReportService(10, 5000));
                services.Add(new MockCustomReportService(10, 5000));
                return new CustomServiceWithMaxRequestCountHandler(services, 5);
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();
            await serviceProvider.GetRequiredService<App>().RunAsync();
        }
    }
}
