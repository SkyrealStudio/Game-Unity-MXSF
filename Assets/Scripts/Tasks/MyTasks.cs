using System.Threading.Tasks;
using UnityEngine;

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

        public async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            if (!Application.isPlaying) return false;
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        public int totalSteps;
        public float totalTime;

        protected int gapTime_ms;
    }
    
    public abstract class VariableTask : IBaseTask, IVariableTask
    {
        public VariableTask()
        {
            _nowTaskPointer = -1;
        }
        public VariableTask(IBaseTask[] _preSelectTasks)
        {
            _nowTaskPointer = -1;
            this._preSelectTasks = _preSelectTasks;
        }

        public IBaseTask Select(int n)
        {
            if (n < 0 || n >= _preSelectTasks.Length) throw new System.Exception("index Out Of Range | Select()");
            _nowTaskPointer = n;
            return _preSelectTasks[n];
        }

        public abstract Task<bool> Execute();
        public abstract Task<bool> Execute_P(IBaseTaskAssemble parentExecuter);

        protected IBaseTask[] _preSelectTasks;
        protected int _nowTaskPointer;
    }

    public abstract class GroupTask : IBaseTask
    {
        public GroupTask(IBaseTask[] tasks)
        {
            this.tasks = tasks;
        }
        public async Task<bool> Execute()
        {
            foreach (IBaseTask iter in tasks)
            {
                bool tb = await iter.Execute();
            }
            return true;
        }

        public async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            bool tb = await Execute();
            if(tb) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        protected IBaseTask[] tasks;
    }
}

public class MyTasks
{
    public class CameraMove_Zoom_001 : MyTasksAbstract.TimeAsyncTask,IBaseTask // zoom and move the camera
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

            controllerLocker.LockFrom(this,ControllerLocker.ControllerLockerState.OnlyInteract);

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

            controllerLocker.LockFrom(this, ControllerLocker.ControllerLockerState.OnlyInteract);

            for (int counter = 0; counter < totalSteps && Application.isPlaying; counter++)
            {
                tarTextBox.imageComponent_Main.color += new Color(0f, 0f, 0f, 1f / totalSteps);
                tarTextBox.textComponent_Main.color += new Color(0f, 0f, 0f, 1f / totalSteps);
                await Task.Delay(gapTime_ms);
                //Debug.Log(counter);
            }

            if (_releaseLockMode && Application.isPlaying)
            {
                controllerLocker.UnLockFrom(this);
            }

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

            controllerLocker.LockFrom(this, ControllerLocker.ControllerLockerState.OnlyInteract);

            for (int counter = 0; counter < totalSteps && Application.isPlaying ; counter++)
            {
                tarTextBox.imageComponent_Main.color += new Color(0f, 0f, 0f, -1f / totalSteps);
                tarTextBox.textComponent_Main.color += new Color(0f, 0f, 0f, -1f / totalSteps);
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

        public TextBox tarTextBox;

        private bool _releaseLockMode;
        public ControllerLocker controllerLocker;
    }

    public class TextBoxTextWork_000 : IBaseTask // clear the textBox
    {
        public TextBoxTextWork_000(TextBox targetTextBox)
        {
            this.targetTextBox = targetTextBox;
        }

        public Task<bool> Execute()
        {
            targetTextBox.ClearText_Main();
            return new Task<bool>(()=> { return true; });
        }

        public async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        public TextBox targetTextBox;
    }

    public class TextBoxTextWork_001 : MyTasksAbstract.TimeAsyncTask, IBaseTask // show sth in the textBox
    {
        public TextBoxTextWork_001(
            TextBox targetTextBox,
            string content,
            int totalSteps,
            float totalTime,
            bool clearBox = false
            ) : base(totalSteps,totalTime)
        {
            this.clearBox = clearBox;
            this.targetTextBox = targetTextBox;
            this.content = content;
            _gapCount_textContent = (float)this.content.Length / (float)totalSteps;
            _currentCount_textContent = 0f;
            _currentCount_Int_textContent = 0;
        }

        public override async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;

            if (clearBox) targetTextBox.ClearText_Main();
            for (int counter = 0;counter<totalSteps && Application.isPlaying; counter++)
            {
                _currentCount_textContent += _gapCount_textContent;
                for(;_currentCount_Int_textContent<(int)_currentCount_textContent;_currentCount_Int_textContent++)
                {
                    if (_currentCount_Int_textContent < (content.Length - 1))
                        targetTextBox.AppendText_Main(content[_currentCount_Int_textContent]);
                    else
                        break;
                }
                
                await Task.Delay(gapTime_ms);
            }
            targetTextBox.SetText_Force_Main(content);//Solve tail

            return true;
        }

        private float _currentCount_textContent;
        private int _currentCount_Int_textContent;
        private float _gapCount_textContent;
        public bool clearBox;
        public string content;
        public TextBox targetTextBox;
    }
    
    public class TextBoxBranchAdjust_001 : MyTasksAbstract.TimeAsyncTask, IBaseTask // start The TextBox's Branch
    {
        public TextBoxBranchAdjust_001(
            ControllerLocker controllerLocker,
            TextBox tarTextBox,
            int branchCount,
            string[] strShow,
            int totalSteps,
            float totalTime
            ) : base(totalSteps, totalTime)
        {
            if (branchCount != strShow.Length) throw new System.Exception("branchCount and strShow.Length does not match");

            this.controllerLocker = controllerLocker;
            this.tarTextBox = tarTextBox;
            this.branchCount = branchCount;

            for (int i = 0; i < branchCount; i++)
                tarTextBox.textComponents_Branch[i].text = strShow[i];
        }

        public override async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;

            controllerLocker.LockFrom(this, ControllerLocker.ControllerLockerState.OnlyNum,branchCount-1);

            for (int i = 0; i < branchCount; i++)
            {
                tarTextBox.textComponents_Branch[i].color = Color.black;
                tarTextBox.textComponents_Branch[i].color *= new Color(1f, 1f, 1f, 0f);

                tarTextBox.imageComponents_Branch[i].color = Color.white;
                tarTextBox.imageComponents_Branch[i].color *= new Color(1f, 1f, 1f, 0f);
            }

            for (int counter = 0; counter < totalSteps && Application.isPlaying; counter++)
            {
                for (int i=0;i<branchCount;i++)
                {
                    tarTextBox.textComponents_Branch[i].color += new Color(0f, 0f, 0f, 1f / totalSteps);
                    tarTextBox.imageComponents_Branch[i].color += new Color(0f, 0f, 0f, 1f / totalSteps);
                }
                await Task.Delay(gapTime_ms);
                //Debug.Log(counter);
            }

            tarTextBox.activatedBranchesCount = branchCount;
             
            return true;
        }

        private ControllerLocker controllerLocker;
        public TextBox tarTextBox;
        public int branchCount;
    }

    public class TextBoxBranchAdjust_002 : MyTasksAbstract.TimeAsyncTask, IBaseTask // start The TextBox's Branch
    {
        public TextBoxBranchAdjust_002(
            ControllerLocker controllerLocker,
            TextBox tarTextBox,
            int totalSteps,
            float totalTime
            ) : base(totalSteps, totalTime)
        {
            this.controllerLocker = controllerLocker;
            this.tarTextBox = tarTextBox;
        }

        public override async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;

            controllerLocker.LockFrom(this, ControllerLocker.ControllerLockerState.OnlyInteract);

            for (int counter = 0; counter < totalSteps && Application.isPlaying; counter++)
            {
                for (int i = 0; i < tarTextBox.activatedBranchesCount; i++)
                {
                    tarTextBox.textComponents_Branch[i].color -= new Color(0f, 0f, 0f, 1f / totalSteps);
                    tarTextBox.imageComponents_Branch[i].color -= new Color(0f, 0f, 0f, 1f / totalSteps);
                }
                await Task.Delay(gapTime_ms);
                //Debug.Log(counter);
            }

            for (int i = 0; i < tarTextBox.activatedBranchesCount; i++)
            {
                tarTextBox.textComponents_Branch[i].color *= new Color(1f,1f,1f,0f);
                tarTextBox.imageComponents_Branch[i].color *= new Color(1f, 1f, 1f, 0f);
            }//tail

            tarTextBox.activatedBranchesCount = 0;
            
            return true;
        }

        private ControllerLocker controllerLocker;
        public TextBox tarTextBox;
    }

    public class TextBoxVariableTask001 : MyTasksAbstract.VariableTask
    {
        public TextBoxVariableTask001(IBaseTask _closeAnimeTask,IBaseTask[] _preSelectTasks) : base(_preSelectTasks)
        {
            this._closeAnimeTask = _closeAnimeTask;
        }

        public void AddTask(IBaseTask task)
        {
            //WatchOut, May cause Bug
            IBaseTask[] trans = new IBaseTask[_preSelectTasks.Length + 1];
            for(int i=0;i< _preSelectTasks.Length;i++)
                trans[i] = _preSelectTasks[i];
            trans[_preSelectTasks.Length] = task;
            _preSelectTasks = trans;
        }

        public override async Task<bool> Execute()
        {
            await _closeAnimeTask.Execute();

            if (_nowTaskPointer == -1) throw new System.Exception("No selected task but you tried to execute it ! | TextBoxVariableTask001.Execute");
            return await _preSelectTasks[_nowTaskPointer].Execute();
        }

        public override async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            bool tb = await Execute();
            if(tb) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        private IBaseTask _closeAnimeTask;
    }

    public class TextBoxGroupTask : MyTasksAbstract.GroupTask
    {
        public TextBoxGroupTask(IBaseTask[] tasks) : base(tasks)
        {
            
        }
    }

    public class Acknowledge_TaskIsComplete : IBaseTask
    {
        public Acknowledge_TaskIsComplete(IInteractBase _interacter)
        {
            this._interacter = _interacter;
        }
        public async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;
            _interacter.InteractIsComplete();
            return true;
        }

        public async Task<bool> Execute_P(IBaseTaskAssemble parentExecuter)
        {
            if (!Application.isPlaying) return false;
            bool completedFlag = await Execute();
            if (completedFlag) parentExecuter.ReleaseExecutingStatus();
            return true;
        }

        private IInteractBase _interacter;
    }
}
