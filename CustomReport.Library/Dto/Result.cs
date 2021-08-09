using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReport.Library.Dto
{
    /// <summary>
    /// Result
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 服務index
        /// </summary>
        public int ServiceIndex { get; set; }

        /// <summary>
        /// 服務資訊
        /// </summary>
        public ServiceInfo Info { get; set; }

        /// <summary>
        /// 回傳內容
        /// </summary>
        public ReportResponse Response { get; set; }
    }
}
