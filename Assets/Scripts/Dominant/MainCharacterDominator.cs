using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyStructures;
using System.Threading.Tasks;

using Interface.Task;
using Interface.Task.Chain;
using Interface.TextParser.ReturnUnit;
using Interface.TextPhraser;

public class MainCharacterDominator : MonoBehaviour, ITaskStructCarrier, ITaskExecuter_Mk001, IVariableTaskExecuter001, ITaskChainNodeCarrier, IParserUnitCarrier, ITaskExecuter_Mk002
{
    private MyStruct1<TaskQueueWithTickCount<IBaseTask>> taskStruct;

    private MyStruct1<DataWithTickCount<Unit_Mk004>> taskEntranceUnit;
    
    private Unit_Mk004 taskChainNode = new Unit_Mk004();

    public bool isExecutingStatus = false;
    public bool isExecuting;

    public void SetTaskChainNode(Unit_Mk004 u)
    {
        taskChainNode = u;
    }

    MyStruct1<DataWithTickCount<Unit_Mk004>> IParserUnitCarrier.GetTaskEntranceStruct()
    {
        return taskEntranceUnit;
    }

    public ref Unit_Mk004 GetTaskChainNode()
    {
        return ref taskChainNode;
    }
    
    public MyStruct1<TaskQueueWithTickCount<IBaseTask>> GetTaskStruct()
    {
        return taskStruct;
    }

    public TaskQueueWithTickCount<IBaseTask> GetTaskStructTop()
    {
        if (taskStruct.Count > 0)
            return taskStruct.Top();
        else
            throw new System.NullReferenceException();
    }

    private void Start()
    {
        isExecuting = false;
        taskStruct = new MyStruct1<TaskQueueWithTickCount<IBaseTask>>();
    }

    async void ITaskExecuter_Mk002.ExecuteTaskAsync(IBaseTask task)
    {
        if (isExecuting) return;
        isExecuting = true;
        await task.ExecuteAsync();
        isExecuting = false;
    }

    public async void ExecuteTask()
    {
        throw new System.Exception("This Method is not allowed to be Invoked");
        if (isExecuting) return;
        isExecuting = true;
        await this.GetTaskStruct().Top().Dequeue().ExecuteAsync();
        isExecuting = false;
    }

    public async void ExecuteVariableTask(int n)
    {
        throw new System.Exception("This Method is not allowed to be Invoked");
        if (isExecuting || !Application.isPlaying) return;
        isExecuting = true;
        IVariableTask variableTask = this.GetTaskStruct().Top().Dequeue() as IVariableTask;
        variableTask.Select(n);
        await variableTask.ExecuteAsync();
        isExecuting = false;
    }

}
