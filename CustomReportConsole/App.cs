using CustomReport.Library;
using CustomReport.Library.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomReportConsole
{
    /// <summary>
    /// app
    /// </summary>
    public class App
    {
        /// <summary>
        /// customservicehandler
        /// </summary>
        private readonly CustomServiceHandler _customServiceHandler;

        /// <summary>
        /// 控制服務最大請求次數customservicehandler
        /// </summary>
        private readonly CustomServiceWithMaxRequestCountHandler _customServiceWithMaxRequestCountHandler;

        /// <summary>
        /// 建構式注入handler服務
        /// </summary>
        /// <param name="customServiceHandler">customservicehandler</param>
        /// <param name="customServiceWithMaxRequestCountHandler">控制服務最大請求次數customservicehandler</param>
        public App(
            CustomServiceHandler customServiceHandler,
            CustomServiceWithMaxRequestCountHandler customServiceWithMaxRequestCountHandler)
        {
            _customServiceHandler = customServiceHandler;
            _customServiceWithMaxRequestCountHandler = customServiceWithMaxRequestCountHandler;
        }

        /// <summary>
        /// 執行方法
        /// </summary>
        /// <returns></returns>
        public Task RunAsync()
        {
            var request = new ReportRequest()
            {
                AssignSpid = "",
                Dtno = 5493,
                Ftno = 0,
                Keymap = "",
                Params = "AssignID=00878;AssignDate=20200101-99999999;DTOrder=2;MTPeriod=2;"
            };
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var result = await _customServiceWithMaxRequestCountHandler.GetDataAsync(request);
                }));
            }

            Task.WaitAll(tasks.ToArray());
            return Task.CompletedTask;
        }
    }
}
