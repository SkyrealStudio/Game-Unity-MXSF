using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;

//----------Interface

public interface IBaseTask
{
    Task<bool> Execute();
    void Execute_P(IBaseTaskAssemble parentExecuter);
}


public interface IBaseTaskAssemble
{ 
    void Execute();
    int Execute_GetCount();
    void ReleaseExecutingStatus();
}