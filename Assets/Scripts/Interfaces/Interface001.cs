using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;

//----------Interface
public class ConnecterTask : IBaseTask
{
    public ConnecterTask()
    {
        _count = 0;
    }
    public ConnecterTask(int n)
    {
        _count = n;
    }
    public async Task<bool> Execute()
    {
        return true;
    }

    public int ConnectCount
    {
        get => _count;
    }

    public async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
    {
        return true;
    }
    private int _count;
}


public interface IBaseTask
{
    Task<bool> Execute();

    //Execute_(with)ParentExecuterIn, used for adjust its executing status
    Task<bool> Execute_P(IBaseTaskAssemble parentExecuter);
}


public interface IBaseTaskAssemble
{ 
    void Execute();
    Task<bool> Execute(int taskCount);
    int Execute_GetCount();
    void ReleaseExecutingStatus();
} 