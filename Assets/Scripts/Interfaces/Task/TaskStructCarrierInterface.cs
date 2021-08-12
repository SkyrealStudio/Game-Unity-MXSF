using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStructures;

using Interface.Task;

namespace Interface.Task
{
    [Obsolete("ycMia 20210810")]
    public interface ITaskStructCarrier
    {
        MyStruct1<TaskQueueWithTickCount<IBaseTask>> GetTaskStruct();
    }

    //[Obsolete("ycMia 20210811")]
    //public interface ITaskEntranceStruct
    //{
    //    MyStruct1<DataWithTickCount<IBaseTask>> GetTaskEntranceStruct();
    //}
}

