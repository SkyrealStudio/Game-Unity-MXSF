using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyNamespace;
using Assets.MyStructures;

public class TaskQueueMethods//单例模式
{
    public static TaskQueueMethods instance = new TaskQueueMethods();
    private TaskQueueMethods() { }

    public TaskQueueMethods handle;
    public void InsertQueueWith(IBaseTask[] tasksIn, MyStruct1<TaskQueueWithTickCount<IBaseTask>> targetStruct)
    {
        TaskQueueWithTickCount<IBaseTask> transfer = new TaskQueueWithTickCount<IBaseTask>(targetStruct.Top().tickCount);
        
        foreach (IBaseTask task in tasksIn)
        {
            transfer.Enqueue(task);
        }
        while(targetStruct.Top().Count>0)
        {
            transfer.Enqueue(targetStruct.Top().Dequeue());
        }
        while(transfer.Count>0)
        {
            targetStruct.Top().Enqueue(transfer.Dequeue());
        }
    }
}
