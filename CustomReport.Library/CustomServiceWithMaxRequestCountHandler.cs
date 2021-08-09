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
        /// 各service lock
        /// </summary>
        private List<SemaphoreSlim> _serviceLockers = new List<SemaphoreSlim>();

        /// <summary>
        /// service index, service request count 對應表
        /// </summary>
        private static ConcurrentDictionary<int, int> _serviceRequestCountMap;

        /// <summary>
        /// locker
        /// </summary>
        private static readonly SemaphoreSlim _locker = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 服務主機
        /// </summary>
        private List<ICustomService> _customServices;

        /// <summary>
        /// 目前handler請求數量
        /// </summary>
        private int _currentRequestCount;

        /// <summary>
        /// 主機最大請求數量
        /// </summary>
        private int _maxRequestCount;

        /// <summary>
        /// 建構式初始化
        /// </summary>
        /// <param name="services">客製報表服務</param>
        /// <param name="maxRequestCount">主機最大請求數量</param>
        public CustomServiceWithMaxRequestCountHandler(List<ICustomService> services, int maxRequestCount)
        {
            _maxRequestCount = maxRequestCount;
            _currentRequestCount = 0;
            _customServices = new List<ICustomService>();
            _customServices.AddRange(services);
            _serviceRequestCountMap = new ConcurrentDictionary<int, int>();
            for (int i = 0; i < _customServices.Count; i++)
            {
                _serviceRequestCountMap.TryAdd(i, 0);
                _serviceLockers.Add(new SemaphoreSlim(maxRequestCount, maxRequestCount));
            }
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="reportRequest">reportRequest</param>
        /// <returns></returns>
        public async Task<ReportResponse> GetDataAsync(ReportRequest reportRequest)
        {
            Interlocked.Increment(ref _currentRequestCount);
            int currentServiceIndex;
            int taskID = _currentRequestCount;

            await _locker.WaitAsync();
            currentServiceIndex = await GetServiceIndexAsync();
            int count;
            var isTrue = _serviceRequestCountMap.TryGetValue(currentServiceIndex, out count);
            Console.WriteLine($"Task {taskID} into Service , currentServiceIndex = {currentServiceIndex}, service request count = {count}");
            _serviceRequestCountMap.AddOrUpdate(currentServiceIndex, _serviceRequestCountMap[currentServiceIndex] + 1, (k, v) => _serviceRequestCountMap[currentServiceIndex] + 1);
            await _serviceLockers[currentServiceIndex].WaitAsync();
            _locker.Release();

            var randomService = _customServices[currentServiceIndex];
            var result = await randomService.GetDataAsync(reportRequest);
            _serviceRequestCountMap.AddOrUpdate(currentServiceIndex, _serviceRequestCountMap[currentServiceIndex] - 1, (k, v) => _serviceRequestCountMap[currentServiceIndex] - 1);
            _serviceLockers[currentServiceIndex].Release();
            Console.WriteLine($"Task {taskID} finish Service, ServiceIndex = {currentServiceIndex}");

            return result;
        }

        /// <summary>
        /// 隨機取得請求服務index
        /// </summary>
        /// <returns>index</returns>
        private async Task<int> GetServiceIndexAsync()
        {
            var idleServicesIndexes = new List<int>();
            for (int i = 0; i < _customServices.Count; i++)
            {
                int currentCount;
                var isTrue = _serviceRequestCountMap.TryGetValue(i, out currentCount);
                if (isTrue && currentCount < _maxRequestCount)
                {
                    idleServicesIndexes.Add(i);
                }
            }

            if (idleServicesIndexes.Count != 0)
            {
                return GetRandonIndex(idleServicesIndexes);
            }

            return await GetIdleServiceIndexAsync();
        }

        /// <summary>
        /// 隨機取得主機index
        /// </summary>
        /// <param name="services">閒置主機</param>
        /// <returns>index</returns>
        private int GetRandonIndex(List<int> services)
        {        
            return services.OrderBy(i => Guid.NewGuid()).First();
        }

        /// <summary>
        /// 取得閒置主機index
        /// </summary>
        /// <returns>index</returns>
        private async Task<int> GetIdleServiceIndexAsync()
        {
            List<Task<int>> tasks = new List<Task<int>>();

            for (var i = 0; i < _customServices.Count; i++)
            {
                var index = i;
                tasks.Add(Task.Run(async () =>
                {
                    var tcs = new TaskCompletionSource<int>();
                    await CheckedServiceIdelAsync(index, tcs);
                    return await tcs.Task;
                }));
            }

            var taskIndex = await Task.WhenAny(tasks.ToArray());

            return await taskIndex;
        }

        /// <summary>
        /// 確認主機是否閒置
        /// </summary>
        /// <param name="conccurrentServiceIndex">服務index</param>
        /// <param name="tcs">TaskCompletionSource</param>
        /// <returns></returns>
        private async Task CheckedServiceIdelAsync(int conccurrentServiceIndex, TaskCompletionSource<int> tcs)
        {
            await _serviceLockers[conccurrentServiceIndex].WaitAsync();
            tcs.SetResult(conccurrentServiceIndex);
            _serviceLockers[conccurrentServiceIndex].Release();
        }
    }
}
