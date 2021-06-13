using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    StringType,
}

public interface IGeneralTask
{
    void Execute();
    TaskType GetTaskType();
}

public interface ITextTask : IGeneralTask
{
    string GetString();
    void Restore();
}
