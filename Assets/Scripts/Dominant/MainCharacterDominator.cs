using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyStructures;
using System.Threading.Tasks;

using Interface.Task;
using Interface.Task.Chain;
using Interface.TextParser.ReturnUnit;

public class MainCharacterDominator : MonoBehaviour, ITaskStructCarrier,ITaskExecuter, IVariableTaskExecuter001, ITaskEntranceStruct, ITaskChainNodeCarrier
{
    private MyStruct1<TaskQueueWithTickCount<IBaseTask>> taskStruct;
    private MyStruct1<DataWithTickCount<IBaseTask>> taskEntranceStruct;

    private Unit_Mk004 taskChainNode;

    public bool isExecutingStatus = false;
    public bool isExecuting;

    public void SetTaskChainNode(Unit_Mk004 u)
    {
        taskChainNode = u;
    }

    public Unit_Mk004 GetTaskChainNode()
    {
        return taskChainNode;
    }

    public MyStruct1<DataWithTickCount<IBaseTask>> GetTaskEntranceStruct()
    {
        return taskEntranceStruct;
    }
    public MyStruct1<TaskQueueWithTickCount<IBaseTask>> GetTaskStruct()
    {
        return taskStruct;
    }
    //public IBaseTask GetTakStructTop_Tail()
    //{
    //    throw new System.NotImplementedException();
    //}
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
    
    public async void ExecuteTask()
    {
        if (isExecuting) return;
        isExecuting = true;
        await this.GetTaskStruct().Top().Dequeue().Execute();
        isExecuting = false;
    }

    public async void ExecuteVariableTask(int n)
    {
        if (isExecuting || !Application.isPlaying) return;
        isExecuting = true;
        IVariableTask variableTask = this.GetTaskStruct().Top().Dequeue() as IVariableTask;
        variableTask.Select(n);
        await variableTask.Execute();
        isExecuting = false;
    }

   
}
