using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.MyStructures;
using System;
using System.Threading.Tasks;

using Interface.Task;

namespace Scripts
{
    public class CameraExecuter : MonoBehaviour
    {
        public Queue<IBaseTask> taskQueue;
        public PersistentObjectManager persistentObjectManager;
        public new Camera camera;

        public class CamMode
        {
            public enum Mode
            {
                RunningMode,
                NormalMode,
                WaitUnitillNextTask,
                Executing,
                //LockMode,
            }
            private Mode mode;

            public void AckTaskStart()
            {
                mode = Mode.RunningMode;
            }
            public void AckExecuting()
            {
                mode = Mode.Executing;
            }
            public void AckTaskEnd()
            {
                mode = Mode.WaitUnitillNextTask;
            }

            public void AckEscape()
            {
                mode = Mode.NormalMode;
            }

            public bool IsTaskMode { get => mode == Mode.RunningMode; }
            public bool IsExecuting { get => mode == Mode.Executing; }
            public bool IsWaitingMode { get => mode == Mode.WaitUnitillNextTask; }
            public bool IsNormal { get => mode == Mode.NormalMode; }

            public CamMode()
            {
                mode = Mode.NormalMode;
            }
        }
        private CamMode camMode;

        public void ReceiveTask(IBaseTask task)
        {
            taskQueue.Enqueue(task);
        }

        private void Awake()
        {
            taskQueue = new TaskQueueWithTickCount<IBaseTask>(persistentObjectManager.GetTickCount());
            camMode = new CamMode();
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
                persistentObjectManager.MainCharacterGObj.transform.position.x,
                persistentObjectManager.MainCharacterGObj.transform.position.y,
                camera.transform.position.z);
            return;
            //throw new NotImplementedException();
        }

        void LateUpdate()
        {
            if (camMode.IsTaskMode)
            {
                if (taskQueue.Count != 0)
                {
                    _TryExecuteTask(taskQueue.Count != 0);
                    camMode.AckExecuting();
                }
                else
                {
                    Debug.LogError("CameraExecuter's taskQueue is Empty but you tried to AckTaskStart() it");
                    return;
                }
            }
            else if (camMode.IsWaitingMode
                 || camMode.IsExecuting)
            {
                return;
            }
            else if (camMode.IsNormal)
            {
                DoNormalAction();
            }
            else
            {
                throw new System.Exception("Unknown camMode");
            }
        }

        private bool _TryExecuteTask(bool allowance)
        {
            if (allowance)
            {
                taskQueue.Dequeue().Execute();
                return true;
            }
            else
                return false;
        }



        #region
        public void AckTaskStart()
        {
            camMode.AckTaskStart();
        }

        public void AckTaskEnd()
        {
            camMode.AckTaskEnd();
        }

        public void AckEscape()
        {
            camMode.AckEscape();
        }
        #endregion
    }
}
