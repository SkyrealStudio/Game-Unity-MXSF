using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;

//----------Interface
public interface IVariableTask : IBaseTask
{
    IBaseTask Select(int n);
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
    int Execute_GetCount();
    void ReleaseExecutingStatus();
} 