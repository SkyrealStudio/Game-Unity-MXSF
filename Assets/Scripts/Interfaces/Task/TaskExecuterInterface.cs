using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Task
{
    [Obsolete("ycMia 20210811")]
    public interface ITaskExecuter_Mk001
    {
        void ExecuteTask();
    }

    public interface ITaskExecuter_Mk002
    {
        void ExecuteTaskAsync(IBaseTask task);
    }
}
