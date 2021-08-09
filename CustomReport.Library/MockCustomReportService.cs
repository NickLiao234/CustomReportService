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
    /// 假自訂報表服務類別
    /// </summary>
    public class MockCustomReportService : ICustomService
    {
        /// <summary>
        /// 最大請求數量
        /// </summary>
        public int MaxRequestCount { get; set; }

        /// <summary>
        /// 回應時間
        /// </summary>
        public int ResponseMilisecond { get; set; }

        /// <summary>
        /// 目前請求數量
        /// </summary>
        private int currentRequestCount;

        /// <summary>
        /// 建構式設定初始值
        /// </summary>
        /// <param name="maxRequestCount">最大請求數量</param>
        /// <param name="responseMilisecond">回應時間</param>
        public MockCustomReportService(int maxRequestCount = 5, int responseMilisecond = 20000)
        {
            MaxRequestCount = maxRequestCount;
            ResponseMilisecond = responseMilisecond;
            currentRequestCount = 0;
        }

        /// <summary>
        /// 取得目前請求數量資訊
        /// </summary>
        /// <returns>ServiceInfo</returns>
        public ServiceInfo GetCurrentRequestCount()
        {
            return new ServiceInfo()
            {
                CurrentRequestCount = currentRequestCount,
                MaxRequestCount = MaxRequestCount
            };
        }

        /// <summary>
        /// 取得回傳資料
        /// </summary>
        /// <param name="reportRequest">ReportRequest</param>
        /// <returns>ReportResponse</returns>
        public async Task<ReportResponse> GetDataAsync(ReportRequest reportRequest)
        {
            if (currentRequestCount >= MaxRequestCount)
            {
                return new ReportResponse
                {
                    IsCompleted = false,
                    IsFaulted = true,
                    Exception = "Over MaxRequestCount",
                    Signature = "",
                    Result = ""
                };
            }

            Interlocked.Increment(ref currentRequestCount);

            await Task.Delay(ResponseMilisecond);

            Interlocked.Decrement(ref currentRequestCount);

            return new ReportResponse
            {
                IsCompleted = true,
                IsFaulted = false,
                Exception = "",
                Signature = "",
                Result = Guid.NewGuid().ToString()
            };
        }

        /// <summary>
        /// 是否閒置
        /// </summary>
        /// <returns></returns>
        public bool IsIdle()
        {
            if (currentRequestCount == MaxRequestCount)
            {
                return false;
            }

            return true;
        }
    }
}
