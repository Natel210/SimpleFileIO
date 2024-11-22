using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileIO.Enum
{
    public enum EWriteItemErrorCode
    {
        Unknown = -1,
        OK = 0,
        AlreadyWriting,
        GetLogItemsException, // An error occurred while receiving log items.
        InvalidNull, // The copied log item is NULL.
        InvalidEmpty, // The copied log item is empty. It is suspected to be a malfunction because it was operated after determining that it was empty in advance.
        TaskRunWritingException, // An error occurred while writing to the actual file.
        TaskRunCancellationTokenException, //Problem with refraining from tokens and task variables during task run operation.
        TaskRunInvalidRootPath, // not root path null



        //int
    }
}
