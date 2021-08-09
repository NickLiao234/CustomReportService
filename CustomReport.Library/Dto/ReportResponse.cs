using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomReport.Library.Dto
{
    /// <summary>
    /// ReportResponse
    /// </summary>
    public class ReportResponse
    {
        /// <summary>
        /// IsCompleted
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// IsFaulted
        /// </summary>
        public bool IsFaulted { get; set; }

        /// <summary>
        /// Signature
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Exception
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        public string Result { get; set; }
    }
}
