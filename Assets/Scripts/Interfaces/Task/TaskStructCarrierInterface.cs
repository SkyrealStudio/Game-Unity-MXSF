using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStructures;

using Interface.Task;

namespace Interface.Task
{
    public interface ITaskStructCarrier
    {
        MyStruct1<TaskQueueWithTickCount<IBaseTask>> GetTaskStruct();
    }
}

