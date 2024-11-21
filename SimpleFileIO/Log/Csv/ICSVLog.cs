using SimpleFileIO.Enum;
using SimpleFileIO.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileIO.Log.Csv
{
    public interface ICSVLog
    {
        PathProperty Property { get; set; }

        bool IsWriting { get; }

        EAddItemErrorCode Add<T>(T logEntry) where T : class;

        EWriteItemErrorCode Write();

        EClearItemErrorCode Clear();
    }
}
