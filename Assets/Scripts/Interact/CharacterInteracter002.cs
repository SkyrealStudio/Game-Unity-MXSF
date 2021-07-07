using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using MyNamespace;

public class CharacterInteracter002 : MonoBehaviour, IInteractBase
{
    public LongLifeObjectManager longLifeObjectManager;

    public MainCharacterDominantor targetDominantor;
    public Camera tarCam;
    public TipCarrier001 tipCarrier;
    public ChoiceFormCarrier choiceFormCarrier;
    private MyTaskFactory myTaskFactory;

    private List<long> tickID_list;

    public bool needFeed;

    public void InteractIsComplete()
    {
        tipCarrier.gameObject.SetActive(false);
        needFeed = false;
        longLifeObjectManager.tipDominator.Adjust();
    }

    private void Start()
    {
        tickID_list = new List<long>();
        needFeed = true;
        myTaskFactory = new MyTaskFactory(longLifeObjectManager, tarCam, this, () => { return judge(2, _dict); });
        myTaskFactory.Load();
    }

    private static string[] _dict = { "test_branch_1", "test_branch_2"};
    private bool[] judge(int branchLength, string[] dict)
    {
        bool[] rev = new bool[branchLength];
        for (int i = 0; i < branchLength; i++)
            if (choiceFormCarrier.choiceForm.Includes(dict[i]))
                rev[i] = false;//hide
            else
                rev[i] = true;//showing allowed

        return rev;
    }

    protected class MyTaskFactory
    {
        public MyTaskFactory(LongLifeObjectManager longLifeObjectManager,Camera tarCam, IInteractBase interactBase, MyTasks.JudgeAction judgeAction1)
        {
            this.judgeAction1 = judgeAction1;
            this.tarCam = tarCam;
            this.longLifeObjectManager = longLifeObjectManager;
            this._interactBase = interactBase;
            _taskArray = new IBaseTask[6,40];
        }
        
        public void Load()
        {
            _indexX = 0;
            _indexY = 0;

            //_indexX == 0
            _taskArray[_indexX, _indexY++] = new MyTasks.CameraMove_Zoom_001(longLifeObjectManager.currentController.locker, tarCam, 0.5f, tarCam.gameObject.transform.position + new Vector3(0f, -1f), 10, 0.7f);
            _taskArray[_indexX, _indexY++] = new MyTasks.TextBoxAdjust_001(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, 20, 0.1f);
            _taskArray[_indexX, _indexY++] = new MyTasks.CameraMove_Zoom_001(longLifeObjectManager.currentController.locker, tarCam, 1f, tarCam.gameObject.transform.position, 10, 0.7f, true);
            _taskArray[_indexX, _indexY++] = new MyTasks.TextBoxAdjust_002(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, 20, 0.1f);

            //StartBranch -- MC 
            //_indexX == 1
            _indexX++; _indexY = 0;
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[3] { "你是谁", "这是哪", "…" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "精神空间？" }, 20, 0.5f);//1
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "我创造的…？" }, 20, 0.5f);//2
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "…" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "什么东西…?" }, 20, 0.5f);//4
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "所以说这里…" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "…" }, 20, 0.5f);

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_001(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[2] { "我该做什么", "为什么你在这里" }, 20, 0.5f, judgeAction1);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "你为什么要帮我" }, 20, 0.5f);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_003(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, new string[1] { "没什么了" }, 20, 0.5f);//9


            //EndBranch -- MC
            //_indexX == 2
            _indexX++; _indexY = 0;
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxBranchAdjust_002(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, 20, 0.5f);

            //Start -- Fox
            //_indexX == 3
            _indexX++; _indexY = 0;
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "...", 40, 0.1f, true);//0

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：比起我是谁，你还记得之前发生了什么吗", 40, 0.2f, true);//1
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：看来你是什么都不记得了呢…那我来大概说明一下", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：…已经懵到说不出话来了吗，那我大概说明一下情况", 40, 0.2f, true);

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：首先，还是先做一下自我介绍，你可以叫我狐，然后你现在存在的空间…你可以理解为你的精神空间。", 40, 0.2f, true);//4
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：本质上其实是更加复杂的东西，你这样理解也更加方便对吧，顺便，这个空间是你制造的。", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：等一下，在我说明的时候请先认真听我说完。", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：会出现这个空间的情况有很多，不过一般都和你的精神状态有关，我先确认一下，你并没有任何记忆对吧。。", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐:（叹气）那就是了，你为了保护自己所以封住了你自己的记忆，也就是说，当你处理完了这部分的威胁，这片空间就会消失。", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：用简单的方式来解释就是你找回了自己的记忆之后就能从这里出去，然后我会负责引导你。", 40, 0.2f, true);//9
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：先把别人的话听完是基本礼仪，请不要再次打断我说话了。", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：这里是你创建的空间，所以这些场景都是与你的记忆有关联的。你可以试着找到大门钥匙从这里离开，说不定会有什么线索。", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：（叹气）谢谢，我已经说完了。我想我已经解释的十分清楚了吧，如果有什么问题可以提问，不过可以的话我不想再解释我已经解释过的问题。", 40, 0.2f, true);//12

            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：我之前说过了吧…找到你的记忆然后离开这里，当然，如果你愿意呆在这里我也没意见。", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：…", 40, 0.2f, true);//14
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：我在这里是为了引导你离开这里。而且…我认为这里除了我就没有正常的生物了，所以相信我才是最正确的选择。", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：你可以理解为利害一致，总之，我不会害你，并且会帮助你离开这个地方。", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：当然，因为是合作，我也希望你能够和我配合…这样双方都会少很多麻烦。", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：还有什么要问的吗？", 40, 0.2f, true);
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：是吗，那么咨询环节就这么结束了。", 40, 0.2f, true);//19
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：好，那么接下来在教室之类的地方走动一下找找线索吧，我在这里等你。", 40, 0.2f, true);//20
            _taskArray[_indexX,_indexY++] = new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox, "狐：…好了，理解完现在的状况了就赶紧去找线索，我说会在这里等你，但务必快一些。", 40, 0.2f, true);

            //AllEnd
            //_indexX == 4
            _indexX++; _indexY = 0;
            _taskArray[_indexX, _indexY++] = new MyTasks.Acknowledge_TaskIsComplete(_interactBase);
        }
        
        public IBaseTask GetTask(int indexX, int indexY)
        {
            return _taskArray[indexX, indexY];
        }

        public static MyTasks.TextBoxGroupTask Pack_GPTsk(IBaseTask a,IBaseTask b)
        {
            return new MyTasks.TextBoxGroupTask(new IBaseTask[2] { a, b });
        }
        public static MyTasks.TextBoxGroupTask Pack_GPTsk(IBaseTask[] tasks)
        {
            return new MyTasks.TextBoxGroupTask(tasks);
        }
        
        public static MyTasks.TextBoxVariableTask001 Pack_TexVarTsk001(IBaseTask a,MyTasks.JudgeAction j,IBaseTask[] b)
        {
            return new MyTasks.TextBoxVariableTask001(a, j, b);
        }

        public static MyTasks.TextBoxVariableTask002 Pack_TexVarTsk002(IBaseTask a, IBaseTask[] b)
        {
            return new MyTasks.TextBoxVariableTask002(a, b);
        }

        public static MyTasks.TaskStructCuterTask001 Pack_CuterTsk001(UnityAction<IBaseTask[]> ua, IBaseTask[] refTsks)
        {
            return new MyTasks.TaskStructCuterTask001(ua, refTsks);
        }

        public Camera tarCam;
        public LongLifeObjectManager longLifeObjectManager;
        private IInteractBase _interactBase;
        private IBaseTask[,] _taskArray;

        private int _indexX;
        private int _indexY;
        private MyTasks.JudgeAction judgeAction1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacterGObj)
            && needFeed)
        {
            tickID_list.Add(longLifeObjectManager.tickRecorder.tickCount);
            //把一个接口实例放入玩家的接口结构中
            MainCharacterDominantor.MytaskAssemble001 tAssemble = new MainCharacterDominantor.MytaskAssemble001(
                longLifeObjectManager.tickRecorder.tickCount,
                tipCarrier);

            // ua : tAssemble.ChangeQueueWith
            // tAssemble.Enqueue();

            tAssemble.Enqueue(MyTaskFactory.Pack_GPTsk(new IBaseTask[]{
                myTaskFactory.GetTask(0, 0), //zoom
                myTaskFactory.GetTask(0, 1), //show Box
                myTaskFactory.GetTask(3, 0)  //show TextInit
            }));

            tAssemble.Enqueue(myTaskFactory.GetTask(1, 0));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[] {
                myTaskFactory.GetTask(3, 1),
                myTaskFactory.GetTask(3, 2),
                myTaskFactory.GetTask(3, 3)
            }));

            tAssemble.Enqueue(myTaskFactory.GetTask(3, 4));
            tAssemble.Enqueue(myTaskFactory.GetTask(1, 1));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[] {
                myTaskFactory.GetTask(3, 5)
            }));

            tAssemble.Enqueue(myTaskFactory.GetTask(1, 2));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                myTaskFactory.GetTask(3,6)
            }));

            tAssemble.Enqueue(myTaskFactory.GetTask(3, 7));
            tAssemble.Enqueue(myTaskFactory.GetTask(1, 3));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                myTaskFactory.GetTask(3,8)
            }));
            
            tAssemble.Enqueue(myTaskFactory.GetTask(1, 4));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                myTaskFactory.GetTask(3,9)
            }));

            tAssemble.Enqueue(myTaskFactory.GetTask(1, 5));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                myTaskFactory.GetTask(3,10)
            }));
            tAssemble.Enqueue(myTaskFactory.GetTask(3, 11));

            tAssemble.Enqueue(myTaskFactory.GetTask(1, 6));
            tAssemble.Enqueue(MyTaskFactory.Pack_TexVarTsk002(myTaskFactory.GetTask(2, 0), new IBaseTask[]{
                myTaskFactory.GetTask(3,12)
            }));

            targetDominantor.taskStack.Push(tAssemble);
            Debug.Log("in: "+gameObject.name);
            longLifeObjectManager.tipDominator.Adjust();



            
        }
    }
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacterGObj) && needFeed)
        {
            try
            {
                if (tickID_list.Contains(targetDominantor.taskStack.Tail().tickID))
                {
                    targetDominantor.taskStack.Dequeue();
                }
                else if (tickID_list.Contains(targetDominantor.taskStack.Top().tickID))
                {
                    targetDominantor.taskStack.PopTop();
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("seems this task have been executed");
            }

            longLifeObjectManager.tipDominator.Adjust();

            Debug.Log("leave: "+gameObject.name);
        }
    }
}
