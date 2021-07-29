using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MyStructures;
using System;

public class CameraExecuter : MonoBehaviour
{
    public Queue<IBaseTask> taskQueue;
    public LongLifeObjectManager longLifeObjectManager;
    public new Camera camera;

    public class TaskMode
    {
        private bool isTaskMode;

        public bool IsTaskMode { get => isTaskMode; set => isTaskMode = value; }
        
        public TaskMode()
        {
            isTaskMode = false;
        }
    }
    private TaskMode camTaskMode;


    public void ReceiveTask(IBaseTask task)
    {
        camTaskMode.IsTaskMode = true;
        taskQueue.Enqueue(task);
        camTaskMode.IsTaskMode = false;
    }
    
    private void Awake()
    {
        taskQueue = new TaskQueueWithTickCount<IBaseTask>(longLifeObjectManager.GetTickCount());
        camTaskMode = new TaskMode();
    }

    void Start()
    {
        
    }

    private void DoNormalAction()
    {
        DoGetLocations();
        DoCalculate();
        DoSneekCamera();
    }

    private void DoGetLocations()
    {
        return;
        //throw new NotImplementedException();
    }

    private void DoCalculate()
    {
        return;
        //throw new NotImplementedException();
    }

    private void DoSneekCamera()
    {
        camera.transform.position = new Vector3(
            longLifeObjectManager.MainCharacterGObj.transform.position.x,
            longLifeObjectManager.MainCharacterGObj.transform.position.y,
            camera.transform.position.z);
        return;
        //throw new NotImplementedException();
    }
    
    void LateUpdate()
    {
        if(camTaskMode.IsTaskMode)
        {
            _ExecuteTask();
        }
        else
        {
            DoNormalAction();
        }
    }


    public void AckExecuteTask()
    {
        camTaskMode.IsTaskMode = true;
    }

    private void _ExecuteTask()
    {
        taskQueue.Dequeue().Execute();
    }
}
