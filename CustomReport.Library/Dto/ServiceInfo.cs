using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReport.Library.Dto
{
    /// <summary>
    /// 服務資訊
    /// </summary>
    public class ServiceInfo
    {
        /// <summary>
        /// 最大請求數量
        /// </summary>
        public int? MaxRequestCount { get; set; }

        /// <summary>
        /// 目前請求數量
        /// </summary>
        public int CurrentRequestCount { get; set; }
    }
}
