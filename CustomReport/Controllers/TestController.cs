using CustomReport.Library;
using CustomReport.Library.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomReport.Controllers
{
    /// <summary>
    /// TestController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 報表服務
        /// </summary>
        private CustomServiceHandler customService;

        /// <summary>
        /// 建構式注入報表服務
        /// </summary>
        /// <param name="customService">報表服務介面</param>
        public TestController(
            CustomServiceHandler customService)
        {
            this.customService = customService;
        }

        /// <summary>
        /// 取得報表服務回傳資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetData")]
        public async Task<IActionResult> GetData()
        {
            var request = new ReportRequest()
            {
                AssignSpid = "",
                Dtno = 5493,
                Ftno = 0,
                Keymap = "",
                Params = "AssignID=00878;AssignDate=20200101-99999999;DTOrder=2;MTPeriod=2;"
            };

            var result = await customService.GetDataAsync(request);

            return Ok(result);
        }
    }
}
