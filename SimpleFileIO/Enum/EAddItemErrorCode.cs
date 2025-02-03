using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleFileIO.Enum
{
    /// <summary>
    /// Error code that occurs when adding an item
    /// </summary>
    public enum EAddItemErrorCode
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// OK
        /// </summary>
        OK,
        /// <summary>
        /// Exception during operation
        /// </summary>
        WorkException,
        /// <summary>
        /// If the previous data and the subsequent data are of different types.
        /// </summary>
        DifferentDataType,
        
    }
}
