using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using MyNamespace;

public class MyTasksAbstract
{
    public abstract class TimeAsyncTask : IBaseTask
    {
        public TimeAsyncTask(int totalSteps, float totalTime)
        {
            this.totalSteps = totalSteps;
            this.totalTime = totalTime;
            gapTime_ms = (int)(this.totalTime * 1000 / totalSteps);
        }

        public abstract Task<bool> Execute();
        public abstract Task<bool> Execute_P(IBaseTaskAssemble parentExecuter);

        public int totalSteps;
        public float totalTime;

        protected int gapTime_ms;
    }
}

public class MyTasks
{
    public class CameraMove_Zoom_001 : MyTasksAbstract.TimeAsyncTask,IBaseTask
    {
        public CameraMove_Zoom_001(
            ControllerLocker controllerLocker,
            Camera tarCam,
            float camSizeProportion,
            Vector3 finalCamPosition,
            int totalSteps,
            float totalTime,
            bool releaseLockMode = false) : base(totalSteps,totalTime)
        {
            this.controllerLocker = controllerLocker;
            this.tarCam = tarCam;
            this.camSizeProportion = camSizeProportion;
            this.finalCamPosition = finalCamPosition;

            _releaseLockMode = releaseLockMode;
            
            this.targetCamSize = tarCam.orthographicSize * camSizeProportion;
        }

        public override async Task<bool> Execute()
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

        public override async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            if (!Application.isPlaying) return false;
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        public Camera tarCam;
        public float camSizeProportion;
        public Vector3 finalCamPosition;

        private float targetCamSize;

        private bool _releaseLockMode;
        public ControllerLocker controllerLocker;
    }

    public class TextBoxAdjust_001 : MyTasksAbstract.TimeAsyncTask, IBaseTask //start The TextBox
    {
        public TextBoxAdjust_001(
            ControllerLocker controllerLocker,
            TextBox tarTextBox,
            int totalSteps,
            float totalTime,
            bool releaseLockMode = false) : base(totalSteps, totalTime)
        {
            this.controllerLocker = controllerLocker;
            this.tarTextBox = tarTextBox;
            _releaseLockMode = releaseLockMode;
            gapTime_ms = (int)(this.totalTime * 1000 / totalSteps);
        }

        public override async Task<bool> Execute()
        {
            tarTextBox.gameObject.SetActive(true);
            if (!Application.isPlaying) return false;
            //防止在游戏退出后更改

            controllerLocker.LockFrom(this);

            for (int counter = 0; counter < totalSteps && Application.isPlaying; counter++)
            {
                tarTextBox.imageComponent.color += new Color(0f, 0f, 0f, 1f / totalSteps);
                tarTextBox.textComponent.color += new Color(0f, 0f, 0f, 1f / totalSteps);
                await Task.Delay(gapTime_ms);
                //Debug.Log(counter);
            }

            if (_releaseLockMode && Application.isPlaying)
            {
                controllerLocker.UnLockFrom(this);
            }

            return true;
        }

        public override async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            if (!Application.isPlaying) return false;
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        public TextBox tarTextBox;

        private bool _releaseLockMode;
        public ControllerLocker controllerLocker;

    }

    public class TextBoxAdjust_002 : MyTasksAbstract.TimeAsyncTask,IBaseTask //close the TextBox
    {
        public TextBoxAdjust_002(
            ControllerLocker controllerLocker,
            TextBox tarTextBox,

            int totalSteps,
            float totalTime,
            bool releaseLockMode = false) : base(totalSteps,totalTime)
        {
            this.controllerLocker = controllerLocker;
            this.tarTextBox = tarTextBox;
            
            _releaseLockMode = releaseLockMode;
        }

        public override async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;
            //防止在游戏退出后更改

            controllerLocker.LockFrom(this);

            for (int counter = 0; counter < totalSteps && Application.isPlaying ; counter++)
            {
                tarTextBox.imageComponent.color += new Color(0f, 0f, 0f, -1f / totalSteps);
                tarTextBox.textComponent.color += new Color(0f, 0f, 0f, -1f / totalSteps);
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

        public override async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            if (!Application.isPlaying) return false;
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        public TextBox tarTextBox;

        private bool _releaseLockMode;
        public ControllerLocker controllerLocker;
    }

    public class TextBoxTextWork_000 : IBaseTask
    {
        public TextBoxTextWork_000(TextBox targetTextBox)
        {
            this.targetTextBox = targetTextBox;
        }

        public async Task<bool> Execute()
        {
            targetTextBox.textComponent.text = "";
            return true;
        }

        public async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        public TextBox targetTextBox;
    }

    public class TextBoxTextWork_001 : MyTasksAbstract.TimeAsyncTask, IBaseTask
    {
        public TextBoxTextWork_001(
            TextBox targetTextBox,
            string content,
            int totalSteps,
            float totalTime
            ) : base(totalSteps,totalTime)
        {
            this.targetTextBox = targetTextBox;
            this.content = content;
            _gapCount_textContent = (float)this.content.Length / (float)totalSteps;
            _currentCount_textContent = 0f;
            _currentCount_Int_textContent = 0;
        }

        public override async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;
            for (int counter = 0;counter<totalSteps && Application.isPlaying; counter++)
            {
                _currentCount_textContent += _gapCount_textContent;
                for(;_currentCount_Int_textContent<_currentCount_textContent;_currentCount_Int_textContent++)
                    targetTextBox.textComponent.text += content[_currentCount_Int_textContent];
                
                await Task.Delay(gapTime_ms);
            }
            targetTextBox.textComponent.text = content;//Solve tail

            return true;
        }

        public override async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            if (!Application.isPlaying) return false;
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        private float _currentCount_textContent;
        private int _currentCount_Int_textContent;
        private float _gapCount_textContent;
        public string content;
        public TextBox targetTextBox;
    }
}
