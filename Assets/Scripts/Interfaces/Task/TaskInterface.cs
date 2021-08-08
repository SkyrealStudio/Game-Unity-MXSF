using System.Collections.Generic;
using System.Threading.Tasks;

using Interface.Task;

namespace Interface.Task
{
    public interface IBaseTask
    {
        Task<bool> Execute();
    }

    public interface IVariableTask : IBaseTask
    {
        IBaseTask Select(int n);
    }
}
