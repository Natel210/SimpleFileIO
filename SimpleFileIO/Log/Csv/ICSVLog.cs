using SimpleFileIO.Enum;
using SimpleFileIO.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileIO.Log.Csv
{
    /// <summary>
    /// Recordable CSV interface.
    /// </summary>
    public interface ICSVLog
    {
        /// <summary>
        /// Path Property 
        /// </summary>
        PathProperty PathProperty { get; set; }

        /// <summary>
        /// Is it being recorded?
        /// </summary>
        bool IsRecording { get; }

        EAddItemErrorCode Add<T>(T logEntry) where T : class;

        EWriteItemErrorCode Write();

        EClearItemErrorCode Clear();
    }
}
