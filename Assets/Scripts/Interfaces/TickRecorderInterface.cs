using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Interface.Tick
{
    interface ITickRecorder
    {
        int GetTickCount();
    }

    interface ITickRecorderInt64
    {
        Int64 GetTickCount();
    }
}
