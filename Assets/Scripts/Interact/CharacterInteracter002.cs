using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using MyNamespace;

public class CharacterInteracter002 : MonoBehaviour
{
    public LongLifeObjectManager longLifeObjectManager;
    
    public MainCharacterDominantor targetDominantor;
    public Camera tarCam;
    
    private class MyTask_CameraMove001 : IBaseTask
    {
        public MyTask_CameraMove001(
            ControllerLocker controllerLocker,
            Camera tarCam,
            float camSizeProportion,
            Vector3 finalCamPosition,
            int totalSteps,
            float totalTime,
            bool releaseLockMode = false)
        {
            this.controllerLocker = controllerLocker;
            this.tarCam = tarCam;
            this.camSizeProportion = camSizeProportion;
            this.finalCamPosition = finalCamPosition;
            this.totalSteps = totalSteps;
            this.totalTime = totalTime;

            _releaseLockMode = releaseLockMode;

            gapTime_ms = (int)(this.totalTime * 1000 / totalSteps);
            this.targetCamSize = tarCam.orthographicSize * camSizeProportion;
        }

        public async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;
            //防止在游戏退出后更改

            controllerLocker.LockFrom(this);

            for(int counter = 0 ; counter<totalSteps;counter++)
            {
                tarCam.orthographicSize = Mathf.Lerp(tarCam.orthographicSize, targetCamSize, 0.5f); 
                tarCam.transform.position = Vector3.Lerp(tarCam.transform.position, finalCamPosition, 0.5f);
                await Task.Delay(gapTime_ms);
            }

            if (_releaseLockMode & (Application.isPlaying))
            {
                controllerLocker.UnLockFrom(this);
            }
            return true;
        }

        public async void Execute_P(IBaseTaskAssemble parentExecuter)
        {
            bool completedFlag =await Execute();
                if(completedFlag)   parentExecuter.ReleaseExecutingStatus();
        }

        public Camera tarCam;
        public float camSizeProportion;
        public Vector3 finalCamPosition;

        public int totalSteps;
        public float totalTime;

        private int gapTime_ms;
        private float targetCamSize;

        private bool _releaseLockMode;
        public ControllerLocker controllerLocker;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacter))
        {
            //把一个接口实例放入玩家的接口结构中
            targetDominantor.taskStack.Push(new MainCharacterDominantor.MytaskAssemble001());

            targetDominantor.taskStack.Peek().Enqueue(new MyTask_CameraMove001(
                longLifeObjectManager.currentController.locker,
                tarCam,
                0.5f,
                tarCam.gameObject.transform.position + new Vector3(0f, -1f),
                20,
                1f
            ));

            targetDominantor.taskStack.Peek().Enqueue(new MyTask_CameraMove001(
                longLifeObjectManager.currentController.locker,
                tarCam,
                1f,
                tarCam.gameObject.transform.position,
                20,
                1f,
                true
            ));

            Debug.Log("in!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacter))
        {
            //targetDominantor.taskStack.Pop();
            Debug.Log("leave!");
        }
    }
}
