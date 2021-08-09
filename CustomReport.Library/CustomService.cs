using CustomReport.Library.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CustomReport.Library
{
    /// <summary>
    /// 自訂報表服務類別
    /// </summary>
    public class CustomService : ICustomService
    {
        /// <summary>
        /// http服務
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// 目前請求數量
        /// </summary>
        private int currentRequestCount;

        /// <summary>
        /// 建構式注入http服務
        /// </summary>
        /// <param name="httpClientFactory">http服務</param>
        public CustomService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            currentRequestCount = 0;
        }

        /// <summary>
        /// 取得報表服務資料
        /// </summary>
        /// <param name="reportRequest">request</param>
        /// <returns>response</returns>
        public async Task<ReportResponse> GetDataAsync(ReportRequest reportRequest)
        {
            Interlocked.Increment(ref currentRequestCount);

            var httpClinet = _httpClientFactory.CreateClient();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            var content = new StringContent(
                JsonSerializer.Serialize(reportRequest, options),
                Encoding.UTF8,
                "application/json");
            var response = await httpClinet.PostAsync("http://192.168.99.95:5000/api/customreport", content);
            var responseData = await JsonSerializer.DeserializeAsync<ReportResponse>(await response.Content.ReadAsStreamAsync(), options);

            Interlocked.Decrement(ref currentRequestCount);

            return responseData;
        }
    }
}
