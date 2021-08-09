using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStructures
{
    public class TaskQueueWithTickCount<T> : Queue<T>
    {
        public TaskQueueWithTickCount(int tickCount):base()
        {
            this.tickCount = tickCount;
        }

        public int tickCount;
    }
}
