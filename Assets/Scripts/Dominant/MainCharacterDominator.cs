using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MyStructures;
using System.Threading.Tasks;

public class MainCharacterDominator : MonoBehaviour, ITaskStructCarrier,ITaskExecuter, IVariableTaskExecuter001
{
    //public Stack<IGeneralTask> generalTaskStack = new Stack<IGeneralTask>();
    //public MyStruct1<MytaskAssemble001> taskStack = new MyStruct1<MytaskAssemble001>();

    //public class MytaskAssemble001 : IBaseTaskAssemble
    //{
    //    public MyNamespace.ITipBase tipCarrier;

    //    public MytaskAssemble001(long tickID, MyNamespace.ITipBase tipCarrier)
    //    {
    //        this.tipCarrier = tipCarrier;
    //        data = new Queue<IBaseTask>();
    //        isExecuting = false;

    //        this.tickID = tickID;
    //    }

    //    public void Enqueue(IBaseTask task)
    //    {
    //        data.Enqueue(task);
    //    }

    //    public int Count
    //    {
    //        get { return data.Count; }
    //    }

    //    public async void Execute()
    //    {
    //        if (isExecuting) return;
    //        if (data.Count == 0) throw new System.Exception("Too less Tasks remaining for Execute | Execute(void)");
    //        else
    //        {
    //            isExecuting = true;
    //            await data.Dequeue().$(this);
    //        }
    //        Debug.Log("All Done");
    //    }

    //    public async void ExecuteVariableTask_Path001(int para)
    //    {
    //        if (isExecuting) return;
    //        if (data.Count == 0) throw new System.Exception("Too less Tasks remaining for Execute | ExecuteVariableTask_Path001(void)");
    //        else
    //        {
    //            isExecuting = true;
    //            IVariableTask current = data.Dequeue() as IVariableTask;
    //            await current.Select(para).$(this);
    //        }
    //    }

    //    public void InsertQueueWith(IBaseTask[] reference)
    //    {
    //        Queue<IBaseTask> t = new Queue<IBaseTask>();
    //        for (int i = 0; i < reference.Length; i++)
    //            t.Enqueue(reference[i]);
    //        for (; data.Count > 0;)
    //        {
    //            t.Enqueue(data.Dequeue());
    //        }
    //        data = t;
    //    }

    //    public void ChangeQueueWith(IBaseTask[] reference)
    //    {
    //        data.Clear();
    //        foreach (IBaseTask iter in reference)
    //            data.Enqueue(iter);
    //    }

    //    int IBaseTaskAssemble.Execute_GetCount()
    //    {
    //        Execute();
    //        return data.Count;
    //    }

    //    public void ReleaseExecutingStatus()
    //    {
    //        isExecuting = false;
    //    }

    //    public long tickID;
    //    public Queue<IBaseTask> data;
    //    //public Queue<IBaseTask> Data { get => _data; set => _data = value; }
    //    public bool isExecuting;
    //}
    
    private MyStruct1<TaskQueueWithTickCount<IBaseTask>> taskStruct;
    public bool isExecutingStatus = false;
    public bool isExecuting;

    public MyStruct1<TaskQueueWithTickCount<IBaseTask>> GetTaskStruct()
    {
        return taskStruct;
    }
    public IBaseTask GetTakStructTop_Tail()
    {
        throw new System.NotImplementedException();
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
    
    public async void ExecuteTask()
    {
        if (isExecuting) return;
        isExecuting = true;
        await this.GetTaskStruct().Top().Dequeue().Execute();
        isExecuting = false;
    }

    public async void ExecuteVariableTask(int n)
    {
        if (isExecuting) return;
        isExecuting = true;
        IVariableTask variableTask = this.GetTaskStruct().Top().Dequeue() as IVariableTask;
        variableTask.Select(n);
        await variableTask.Execute();
        isExecuting = false;
    }
}
