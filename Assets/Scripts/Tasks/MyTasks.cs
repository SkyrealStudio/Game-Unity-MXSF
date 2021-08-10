using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Interface.Task;
using Interface.Tick;
using Interface;
using Interface.Task.Chain;

using Scripts;

using System.Collections.Generic;
using MyStructures;

namespace MyTasksAbstract
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
            //return _preSelectTasks[_nowTaskPointer];
            return this;
        }

        public abstract Task<bool> Execute();
        

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
        
        protected IBaseTask[] tasks;
    }
}

namespace MyTasks
{
    public class TaskChainEntrance : IBaseTask
    {
        public TaskChainEntrance(ControllerLocker cl,Interface.TextParser.ReturnUnit.Unit_Mk004 unit, ITaskChainNodeCarrier nodeCarrier)
        {
            this.cl = cl;
            this.unit = unit;
            this.nodeCarrier = nodeCarrier;
        }
        public async Task<bool> Execute()
        {
            cl.LockFrom(this, ControllerLocker.ControllerLockerState.OnlyInteract_InChain);
            nodeCarrier.SetTaskChainNode(unit);
            return true;
        }

        public Interface.TextParser.ReturnUnit.Unit_Mk004 unit;
        private ITaskChainNodeCarrier nodeCarrier;
        private ControllerLocker cl;
    }
}


//later than 20210810
namespace MyTasks
{
    public delegate bool[] JudgeAction();
    public delegate bool[] JudgeAction<T0>(T0 arg0);
    
    public class Camera_Messenger_Task001 : IBaseTask
    {
        public Camera_Messenger_Task001(
            CameraExecuter cameraExecuter,
            float camSizeProportion,
            Vector3 finalCamPosition,
            int totalSteps,
            float totalTime,
            bool autoEscape = false
            )
        {
            this.cameraExecuter = cameraExecuter;
            this.camSizeProportion = camSizeProportion;
            this.finalCamPosition = finalCamPosition;
            this.totalSteps = totalSteps;
            this.totalTime = totalTime;
            this.autoEscape = autoEscape;
        }

        public async Task<bool> Execute()
        {
            CameraMove_Zoom_002 task = new CameraMove_Zoom_002(
                cameraExecuter,
                cameraExecuter.camera,
                camSizeProportion,
                finalCamPosition,
                totalSteps,
                totalTime,
                autoEscape
            );
            cameraExecuter.taskQueue.Enqueue(task);
            cameraExecuter.AckTaskStart();
            return true;
        }

        private CameraExecuter cameraExecuter;
        private float camSizeProportion;
        private Vector3 finalCamPosition;
        private int totalSteps;
        private float totalTime;
        private bool autoEscape;
    }

    public class ControllerLockerAction001 : IBaseTask
    {
        public ControllerLockerAction001(
            ControllerLocker controllerLocker,
            ControllerLocker.ControllerLockerState stateIn
            )
        {
            this.controllerLocker = controllerLocker;
            this.stateIn = stateIn;
        }
        
        public async Task<bool> Execute()
        {
            controllerLocker.LockFrom(this, stateIn);
            return true;
        }

        public ControllerLocker controllerLocker;
        public ControllerLocker.ControllerLockerState stateIn;
    }

    [Obsolete("弃用的方法20210727 \n By: ycMia")]
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
                if (!Application.isPlaying) return false;
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

    public class CameraMove_Zoom_002 : MyTasksAbstract.TimeAsyncTask, IBaseTask
    {
        public CameraMove_Zoom_002(
            CameraExecuter cameraExecuterReValueDestination,
            Camera tarCam,
            float camSizeProportion,
            Vector3 finalCamPosition,
            int totalSteps,
            float totalTime,
            bool autoEscape = false
            ) : base(totalSteps, totalTime)
        {
            this.cameraExecuterReValueDestination = cameraExecuterReValueDestination;
            this.tarCam = tarCam;
            this.camSizeProportion = camSizeProportion;
            this.finalCamPosition = finalCamPosition;
            this.targetCamSize = tarCam.orthographicSize * camSizeProportion;
            this.autoEscape = autoEscape;
        }

        public override async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;
            //防止在游戏退出后更改
            for (int counter = 0; counter < totalSteps && Application.isPlaying; counter++)
            {
                tarCam.orthographicSize = Mathf.Lerp(tarCam.orthographicSize, targetCamSize, 0.5f);
                tarCam.transform.position = Vector3.Lerp(tarCam.transform.position, finalCamPosition, 0.5f);
                await Task.Delay(gapTime_ms);
                if (!Application.isPlaying) return false;
            }
            cameraExecuterReValueDestination.AckTaskEnd();
            if (autoEscape)
                cameraExecuterReValueDestination.AckEscape();
            return true;
        }
        private CameraExecuter cameraExecuterReValueDestination;
        private Camera tarCam;
        private float camSizeProportion;
        private Vector3 finalCamPosition;
        private float targetCamSize;
        private bool autoEscape;
    }


    public class TextBoxAdjust_001 : MyTasksAbstract.TimeAsyncTask, IBaseTask //start The TextBox
    {
        public TextBoxAdjust_001(
            ControllerLocker controllerLocker,
            TextBox tarTextBox,
            int totalSteps,
            float totalTime,
            bool _clearTextMode = true,
            bool releaseLockMode = false) : base(totalSteps, totalTime)
        {
            this._clearTextMode = _clearTextMode;
            this.controllerLocker = controllerLocker;
            this.tarTextBox = tarTextBox;
            _releaseLockMode = releaseLockMode;
            gapTime_ms = (int)(this.totalTime * 1000 / totalSteps);
        }

        public override async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;
            tarTextBox.gameObject.SetActive(true);
            //防止在游戏退出后更改

            if(_clearTextMode)
            {
                tarTextBox.ClearText_Main();
            }

            controllerLocker.LockFrom(this, ControllerLocker.ControllerLockerState.OnlyInteract);

            for (int counter = 0; counter < totalSteps && Application.isPlaying; counter++)
            {
                tarTextBox.imageComponent_Main.color += new Color(0f, 0f, 0f, 1f / totalSteps);
                tarTextBox.textComponent_Main.color += new Color(0f, 0f, 0f, 1f / totalSteps);
                await Task.Delay(gapTime_ms);
                if (!Application.isPlaying) return false;
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
        private bool _clearTextMode;
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
                if (!Application.isPlaying) return false;
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

    //20210809 ycMia突击记事
    //这是我玩日麻的第二次和
    //然后直接把rain国了个56000分
    //一条 九条 一筒 九筒 一万 九万 东 南 西 北 中 发财 白板 + 九条 
    //23333333333

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
                if (!Application.isPlaying) return false;
            }
            targetTextBox.SetText_Force_Main(content);//Solve tail

            _currentCount_textContent = 0f;
            _currentCount_Int_textContent = 0;

            return true;
        }

        private float _currentCount_textContent;
        private int _currentCount_Int_textContent;
        private float _gapCount_textContent;
        public bool clearBox;
        public string content;
        public TextBox targetTextBox;
    }

    public class TextBoxBranchAdjust_003 : MyTasksAbstract.TimeAsyncTask, IBaseTask
    {
        public TextBoxBranchAdjust_003(
            ControllerLocker controllerLocker,
            TextBox tarTextBox,
            string[] _strShow,
            int totalSteps,
            float totalTime
            
            ) : base(totalSteps, totalTime)
        {
            this.controllerLocker = controllerLocker;
            this.tarTextBox = tarTextBox;
            this._strShow = _strShow;
            
            branchCount = _strShow.Length;

        }

        public override async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;
            
            for (int i=0;i<_strShow.Length;i++)
                tarTextBox.branchOperation.AddBranch(_strShow[i]);
            
            controllerLocker.LockFrom(this, ControllerLocker.ControllerLockerState.OnlyNum, branchCount - 1);

            for (int i = 0; i < branchCount; i++)
            {
                tarTextBox.textComponents_Branch[i].color = Color.black;
                tarTextBox.textComponents_Branch[i].color *= new Color(1f, 1f, 1f, 0f);

                tarTextBox.imageComponents_Branch[i].color = Color.white;
                tarTextBox.imageComponents_Branch[i].color *= new Color(1f, 1f, 1f, 0f);
            }

            for (int counter = 0; counter < totalSteps && Application.isPlaying; counter++)
            {
                for (int i = 0; i < branchCount; i++)
                {
                    tarTextBox.textComponents_Branch[i].color += new Color(0f, 0f, 0f, 1f / totalSteps);
                    tarTextBox.imageComponents_Branch[i].color += new Color(0f, 0f, 0f, 1f / totalSteps);
                }
                await Task.Delay(gapTime_ms);
                if (!Application.isPlaying) return false;
                //Debug.Log(counter);
            }

            return true;
        }

        private ControllerLocker controllerLocker;
        private string[] _strShow;
        public TextBox tarTextBox;
        public int branchCount;
    }

    public class TextBoxBranchAdjust_001 : MyTasksAbstract.TimeAsyncTask, IBaseTask // show The TextBox's Branch
    {
        public TextBoxBranchAdjust_001(
            ControllerLocker controllerLocker,
            TextBox tarTextBox,
            string[] _strShow,
            int totalSteps,
            float totalTime,
            JudgeAction judgeAction
            ) : base(totalSteps, totalTime)
        {
            this.controllerLocker = controllerLocker;
            this.tarTextBox = tarTextBox;
            this._strShow = _strShow;

            this.judgeAction = judgeAction;
            
            if (judgeAction.Invoke().Length != _strShow.Length)
                throw new System.Exception("judgeResult's Length does not match _strShow.Length | TextBoxBranchAdjust_001(...)");
            this.branchCount = _strShow.Length;

        }

        public override async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;

            bool[] _judgeResult = judgeAction.Invoke();
            for (int i = 0; i < _judgeResult.Length; i++)
                if (_judgeResult[i])
                    tarTextBox.branchOperation.AddBranch(_strShow[i]);
                else
                    branchCount--;
                
            

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
                if (!Application.isPlaying) return false;
                //Debug.Log(counter);
            }
            
            return true;
        }

        private ControllerLocker controllerLocker;
        private string[] _strShow;
        private JudgeAction judgeAction;
        public TextBox tarTextBox;
        public int branchCount;
    }
    
    public class TextBoxBranchAdjust_002 : MyTasksAbstract.TimeAsyncTask, IBaseTask // hide The TextBox's Branch
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
                    if (!Application.isPlaying) return false;
                    tarTextBox.textComponents_Branch[i].color -= new Color(0f, 0f, 0f, 1f / totalSteps);
                    tarTextBox.imageComponents_Branch[i].color -= new Color(0f, 0f, 0f, 1f / totalSteps);
                }
                await Task.Delay(gapTime_ms);
                if (!Application.isPlaying) return false;
                //Debug.Log(counter);
            }

            for (int i = 0; i < tarTextBox.activatedBranchesCount; i++)
            {
                if (!Application.isPlaying) return false;
                tarTextBox.textComponents_Branch[i].color *= new Color(1f,1f,1f,0f);
                tarTextBox.imageComponents_Branch[i].color *= new Color(1f, 1f, 1f, 0f);
            }//tail
            
            tarTextBox.branchOperation.ClearBranchs();
            
            return true;
        }

        private ControllerLocker controllerLocker;
        public TextBox tarTextBox;
    }

    public class TextBoxVariableTask002 : MyTasksAbstract.VariableTask
    {
        public TextBoxVariableTask002(
            IBaseTask _closeAnimeTask,
            IBaseTask[] _preSelectTasks) : base(_preSelectTasks)
        {
            this._closeAnimeTask = _closeAnimeTask;
        }

        [Obsolete("WatchOut, May cause Bug", false)]
        public void AddTask(IBaseTask task)
        {
            IBaseTask[] trans = new IBaseTask[_preSelectTasks.Length + 1];
            for (int i = 0; i < _preSelectTasks.Length; i++)
                trans[i] = _preSelectTasks[i];
            trans[_preSelectTasks.Length] = task;
            _preSelectTasks = trans;
        }

        public override async Task<bool> Execute()
        {
            if (!Application.isPlaying) return false;
            await _closeAnimeTask.Execute();
            if (_nowTaskPointer == -1) throw new System.Exception("No selected task but you tried to execute it ! | TextBoxVariableTask001.Execute");
            return await _preSelectTasks[_nowTaskPointer].Execute();
        }
        
        private IBaseTask _closeAnimeTask;
    }

    public class TextBoxVariableTask001 : MyTasksAbstract.VariableTask
    {
        public TextBoxVariableTask001(
            IBaseTask _closeAnimeTask,
            JudgeAction judgeAction,
            IBaseTask[] _preSelectTasks) : base(_preSelectTasks)
        {
            this.judgeAction = judgeAction;
            this._closeAnimeTask = _closeAnimeTask;
        }

        [Obsolete("WatchOut, May cause Bug", false)]
        public void AddTask(IBaseTask task)
        {
            IBaseTask[] trans = new IBaseTask[_preSelectTasks.Length + 1];
            for(int i=0;i< _preSelectTasks.Length;i++)
                trans[i] = _preSelectTasks[i];
            trans[_preSelectTasks.Length] = task;
            _preSelectTasks = trans;
        }

        public override async Task<bool> Execute()
        {
            await _closeAnimeTask.Execute();

            bool[] judgeResult = judgeAction.Invoke();
            int taskLeftCount = 0;
            for(int i=0;i<judgeResult.Length;i++)
            {
                taskLeftCount += judgeResult[i] ? 1 : 0;
            }
            if(taskLeftCount < _preSelectTasks.Length)//simulation
            {
                IBaseTask[] trans = new IBaseTask[taskLeftCount];
                for (int i=0,counter = 0;i< judgeResult.Length;i++)
                    if(judgeResult[i])
                    {
                        trans[counter] = _preSelectTasks[i];
                        counter++;
                    }
                _preSelectTasks = trans;
            }
            if (_nowTaskPointer == -1) throw new System.Exception("No selected task but you tried to execute it ! | TextBoxVariableTask001.Execute");
            return await _preSelectTasks[_nowTaskPointer].Execute();
        }
        

        private IBaseTask _closeAnimeTask;
        public JudgeAction judgeAction;
    }

    public class TextBoxGroupTask : MyTasksAbstract.GroupTask,IBaseTask
    {
        public TextBoxGroupTask(IBaseTask[] tasks) : base(tasks)
        {
            
        }
    }

    public class ChoiceMarkerTask : IBaseTask
    {
        public ChoiceMarkerTask(ChoiceForm _tarChooseForm,string _choiceNote)
        {
            this._choiceNote = _choiceNote;
            this._tarChooseForm = _tarChooseForm;
        }
        public async Task<bool> Execute()
        {
            _tarChooseForm.Choose(_choiceNote);
            return true;
        }
        
        private string _choiceNote;
        private ChoiceForm _tarChooseForm;
    }

    //Constructing!!!
    //public class TaskStructModifierTask001 : IBaseTask
    //{
    //    //public TaskStructCuterTask001(Queue<IBaseTask> _operateQueue, Queue<IBaseTask> _referenceQueue)
    //    //{
    //    //    this._operateQueue = _operateQueue;
    //    //    this._referenceQueue = _referenceQueue;
    //    //}

    //    public TaskStructModifierTask001(UnityAction<IBaseTask[]> ua, IBaseTask[] _referenceArray)
    //    {
    //        this.ua = ua;
    //        this._referenceArray = _referenceArray;
    //        _referenceQueue = new Queue<IBaseTask>();
    //        foreach (IBaseTask iter in _referenceArray)
    //        {
    //            _referenceQueue.Enqueue(iter);
    //        }
    //    }

    //    public async Task<bool> Execute()
    //    {
    //        ua.Invoke(_referenceArray);
    //        return true;
    //    }

    //    private UnityAction<IBaseTask[]> ua;
    //    private Queue<IBaseTask> _referenceQueue;
    //    private IBaseTask[] _referenceArray;
    //}
    
    //Constructing!!!
    public class TaskStructModifierTask001 : IBaseTask
    {
        //public TaskStructCuterTask001(Queue<IBaseTask> _operateQueue, Queue<IBaseTask> _referenceQueue)
        //{
        //    this._operateQueue = _operateQueue;
        //    this._referenceQueue = _referenceQueue;
        //}

        public TaskStructModifierTask001(UnityAction<IBaseTask[], MyStruct1<TaskQueueWithTickCount<IBaseTask>>> ua, IBaseTask[] _referenceArray, MyStruct1<TaskQueueWithTickCount<IBaseTask>> targetTaskStruct)
        {
            this.ua = ua;
            this._referenceArray = _referenceArray;
            this.targetTaskStruct = targetTaskStruct;
            _referenceQueue = new Queue<IBaseTask>();
            foreach (IBaseTask iter in _referenceArray)
            {
                _referenceQueue.Enqueue(iter);
            }
        }

        public async Task<bool> Execute()
        {
            ua.Invoke(_referenceArray, targetTaskStruct);
            return true;
        }

        private UnityAction<IBaseTask[], MyStruct1<TaskQueueWithTickCount<IBaseTask>>> ua;
        private Queue<IBaseTask> _referenceQueue;
        private IBaseTask[] _referenceArray;
        private MyStruct1<TaskQueueWithTickCount<IBaseTask>> targetTaskStruct;
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
        

        private IInteractBase _interacter;
    }
}
