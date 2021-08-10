using CustomReport.Library.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomReport.Library
{
    /// <summary>
    /// 控制主機類別
    /// </summary>
    public class CustomServiceWithMaxRequestCountHandler : ICustomService
    {
        /// <summary>
        /// serviceIndex佇列
        /// </summary>
        private ConcurrentQueue<int> _queue;

        /// <summary>
        /// locker
        /// </summary>
        private SemaphoreSlim _locker;

        /// <summary>
        /// 服務主機
        /// </summary>
        private List<ICustomService> _customServices;

        /// <summary>
        /// 建構式初始化
        /// </summary>
        /// <param name="services">客製報表服務</param>
        /// <param name="maxRequestCount">主機最大請求數量</param>
        public CustomServiceWithMaxRequestCountHandler(List<ICustomService> services, int maxRequestCount)
        {
            _customServices = new List<ICustomService>();
            _customServices.AddRange(services);
            _locker = new SemaphoreSlim(maxRequestCount * services.Count);
            _queue = new ConcurrentQueue<int>();
            InitQueue(maxRequestCount, services.Count);
        }

        /// <summary>
        /// 隨機新增serviceIndex至佇列
        /// </summary>
        /// <param name="maxRequest">最大請求次數</param>
        /// <param name="serviceCount">service數量</param>
        private void InitQueue(int maxRequest, int serviceCount)
        {
            var rnd = new Random();
            var countMap = new List<int>();
            for (int i = 0; i < serviceCount; i++)
            {
                countMap.Add(maxRequest);
            }

            for (int i = 0; i < serviceCount * maxRequest; i++)
            {
                int index = rnd.Next(0, serviceCount);
                while (countMap[index] == 0)
                {
                    index = rnd.Next(0, serviceCount);
                }
                _queue.Enqueue(index);
                countMap[index] = countMap[index] - 1;
            }
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="reportRequest">reportRequest</param>
        /// <returns></returns>
        public async Task<ReportResponse> GetDataAsync(ReportRequest reportRequest)
        {
            var taskID = Task.CurrentId;

            await _locker.WaitAsync();

            _queue.TryDequeue(out int serviceIndex);
            Console.WriteLine($"TaskID {taskID} get index {serviceIndex}");

            var result = await _customServices[serviceIndex].GetDataAsync(reportRequest);
            _queue.Enqueue(serviceIndex);

            Console.WriteLine($"TaskID {taskID} finish index {serviceIndex}");

            _locker.Release();

            return result;
        }
    }
}
