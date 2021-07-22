using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.MyStructures;

public interface ITaskStructCarrier
{
    MyStruct1<TaskQueueWithTickCount<IBaseTask>> GetTaskStruct();
}

