using CustomReport.Library.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomReport.Library
{
    /// <summary>
    /// 報表服務handeler類別
    /// </summary>
    public class CustomServiceHandler : ICustomService
    {
        /// <summary>
        /// 服務主機
        /// </summary>
        private List<ICustomService> customServices;

        /// <summary>
        /// 目前handler請求數量
        /// </summary>
        private int currentRequestCount;

        /// <summary>
        /// 建構式注入http服務
        /// </summary>
        /// <param name="services">客製報表服務</param>
        public CustomServiceHandler(IEnumerable<ICustomService> services)
        {
            currentRequestCount = 0;
            customServices = new List<ICustomService>();
            customServices.AddRange(services);
        }

        /// <summary>
        /// 取得服務報表回傳結果
        /// </summary>
        /// <param name="reportRequest">reportrequest</param>
        /// <returns>Result</returns>
        public async Task<ReportResponse> GetDataAsync(ReportRequest reportRequest)
        {
            Interlocked.Increment(ref currentRequestCount);
            int taskID = currentRequestCount;
            var currentServiceIndex = GetServiceIndex();

            Console.WriteLine($"Task {taskID} uses Service {currentServiceIndex}");

            var randomService = customServices[currentServiceIndex];
            var result = await randomService.GetDataAsync(reportRequest);

            Console.WriteLine($"Task {taskID} finished, isComplete = {result.IsCompleted}, result = {result.Result}");

            return result;
        }
        
        /// <summary>
        /// 隨機取得請求服務index
        /// </summary>
        /// <returns>index</returns>
        private int GetServiceIndex()
        {
            return GetRandonIndex(customServices);
        }

        /// <summary>
        /// 隨機取得主機index
        /// </summary>
        /// <param name="services">閒置主機</param>
        /// <returns>index</returns>
        private int GetRandonIndex(List<ICustomService> services)
        {
            var rnd = new Random();

            return rnd.Next(0, services.Count);
        }
    }
}
