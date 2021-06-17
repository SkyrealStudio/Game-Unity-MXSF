using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using MyNamespace;

public class MyTasks
{
    public class CameraMove_Zoom_001 : IBaseTask
    {
        public CameraMove_Zoom_001(
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

            for (int counter = 0; counter < totalSteps && Application.isPlaying; counter++)
            {
                tarCam.orthographicSize = Mathf.Lerp(tarCam.orthographicSize, targetCamSize, 0.5f);
                tarCam.transform.position = Vector3.Lerp(tarCam.transform.position, finalCamPosition, 0.5f);
                await Task.Delay(gapTime_ms);
            }

            if (_releaseLockMode && (Application.isPlaying))
            {
                controllerLocker.UnLockFrom(this);
            }
            return true;
        }

        public async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            if (!Application.isPlaying) return false;
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
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

    public class TextBoxAdjust_001 : IBaseTask //start The TextBox
    {
        public TextBoxAdjust_001(
            ControllerLocker controllerLocker,
            TextBox tarTextBox,

            int totalSteps,
            float totalTime,
            bool releaseLockMode = false)
        {
            this.controllerLocker = controllerLocker;
            this.tarTextBox = tarTextBox;
            this.totalSteps = totalSteps;
            this.totalTime = totalTime;

            _releaseLockMode = releaseLockMode;
            gapTime_ms = (int)(this.totalTime * 1000 / totalSteps);
        }

        public async Task<bool> Execute()
        {
            tarTextBox.gameObject.SetActive(true);
            if (!Application.isPlaying) return false;
            //防止在游戏退出后更改

            controllerLocker.LockFrom(this);

            for (int counter = 0; counter < totalSteps && Application.isPlaying; counter++)
            {
                tarTextBox.image_bg.color += new Color(0f, 0f, 0f, 1f / totalSteps);
                tarTextBox.text_context.color += new Color(0f, 0f, 0f, 1f / totalSteps);
                await Task.Delay(gapTime_ms);
                //Debug.Log(counter);
            }

            if (_releaseLockMode && Application.isPlaying)
            {
                controllerLocker.UnLockFrom(this);
            }

            return true;
        }

        public async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            if (!Application.isPlaying) return false;
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        public TextBox tarTextBox;

        public int totalSteps;
        public float totalTime;

        private int gapTime_ms;

        private bool _releaseLockMode;
        public ControllerLocker controllerLocker;

    }

    public class TextBoxAdjust_002 : IBaseTask //close the TextBox
    {
        public TextBoxAdjust_002(
            ControllerLocker controllerLocker,
            TextBox tarTextBox,

            int totalSteps,
            float totalTime,
            bool releaseLockMode = false)
        {
            this.controllerLocker = controllerLocker;
            this.tarTextBox = tarTextBox;
            this.totalSteps = totalSteps;
            this.totalTime = totalTime;

            _releaseLockMode = releaseLockMode;
            gapTime_ms = (int)(this.totalTime * 1000 / totalSteps);
        }

        public async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;
            //防止在游戏退出后更改

            controllerLocker.LockFrom(this);

            for (int counter = 0; counter < totalSteps && Application.isPlaying ; counter++)
            {
                tarTextBox.image_bg.color += new Color(0f, 0f, 0f, -1f / totalSteps);
                tarTextBox.text_context.color += new Color(0f, 0f, 0f, -1f / totalSteps);
                await Task.Delay(gapTime_ms);
                //Debug.Log(counter);
            }

            tarTextBox.gameObject.SetActive(false);

            if (_releaseLockMode && Application.isPlaying)
            {
                controllerLocker.UnLockFrom(this);
            }

            return true;
        }

        public async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            if (!Application.isPlaying) return false;
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        public TextBox tarTextBox;

        public int totalSteps;
        public float totalTime;

        private int gapTime_ms;

        private bool _releaseLockMode;
        public ControllerLocker controllerLocker;

    }

}

