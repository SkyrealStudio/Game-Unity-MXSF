using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;

public interface IBaseTask
{
    Task<bool> Execute();
}

public interface IVariableTask : IBaseTask
{
    IBaseTask Select(int n);
}

//public interface IBaseTaskAssemble
//{
//    void Execute();
//    int Execute_GetCount();
//    void ReleaseExecutingStatus();
//} 