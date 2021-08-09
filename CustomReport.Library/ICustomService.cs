using CustomReport.Library.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReport.Library
{
    /// <summary>
    /// 報表服務介面
    /// </summary>
    public interface ICustomService
    {
        /// <summary>
        /// 取得報表服務回傳
        /// </summary>
        /// <param name="reportRequest">reportrequest</param>
        /// <returns>Result</returns>
        public Task<ReportResponse> GetDataAsync(ReportRequest reportRequest);
    }
}
