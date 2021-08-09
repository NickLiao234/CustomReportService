using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReport.Library.Dto
{
    /// <summary>
    /// ReportRequest
    /// </summary>
    public class ReportRequest
    {
        /// <summary>
        /// Dtno
        /// </summary>
        public int Dtno { get; set; }

        /// <summary>
        /// Ftno
        /// </summary>
        public int Ftno { get; set; }

        /// <summary>
        /// Params
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// Keymap
        /// </summary>
        public string Keymap { get; set; }

        /// <summary>
        /// AssignSpid
        /// </summary>
        public string AssignSpid { get; set; }
    }
}
