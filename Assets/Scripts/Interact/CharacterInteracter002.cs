using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using MyNamespace;

public class CharacterInteracter002 : MonoBehaviour,IInteractBase
{
    public LongLifeObjectManager longLifeObjectManager;
    
    public MainCharacterDominantor targetDominantor;
    public Camera tarCam;
    public TipCarrier001 tipCarrier;

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
            
            tAssemble.Enqueue(new MyTasks.TextBoxGroupTask(
                new IBaseTask[] {
                    new MyTasks.CameraMove_Zoom_001(
                        longLifeObjectManager.currentController.locker,
                        tarCam,
                        0.5f,
                        tarCam.gameObject.transform.position + new Vector3(0f, -1f),
                        10,
                        0.7f),

                    new MyTasks.TextBoxAdjust_001(
                        longLifeObjectManager.currentController.locker,
                        longLifeObjectManager.textBox,
                        20,
                        0.1f)
                })
            );

            tAssemble.Enqueue(
                new MyTasks.TextBoxBranchAdjust_001(
                    longLifeObjectManager.currentController.locker,
                    longLifeObjectManager.textBox,
                    3,
                    new string[3] { "中文","English", "русский" },
                    20,
                    0.5f
                    )
                );

            tAssemble.Enqueue(
                new MyTasks.TextBoxVariableTask001(
                    new MyTasks.TextBoxBranchAdjust_002(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, 20, 0.5f),
                    new IBaseTask[3] 
                    {
                        new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"这是第一个选项",40,0.2f,true),
                        new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"this is the second choice",40,0.2f,true),
                        new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"Я слышал, вы выбрали три варианта",40,0.2f,true)
                    }
                ));

            tAssemble.Enqueue(
                new MyTasks.TextBoxBranchAdjust_001(
                    longLifeObjectManager.currentController.locker,
                    longLifeObjectManager.textBox,
                    3,
                    new string[3] { "中文", "English", "русский" },
                    20,
                    0.5f
                    )
                );

            tAssemble.Enqueue(
                new MyTasks.TextBoxVariableTask001(
                    new MyTasks.TextBoxBranchAdjust_002(longLifeObjectManager.currentController.locker, longLifeObjectManager.textBox, 20, 0.5f),
                    new IBaseTask[3] 
                    {
                        new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"这是第一个选项",40,0.2f,true),
                        new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"this is the second choice",40,0.2f,true),
                        new MyTasks.TextBoxTextWork_001(longLifeObjectManager.textBox,"Я слышал, вы выбрали три варианта",40,0.2f,true)
                    }
                ));
            
            tAssemble.Enqueue(new MyTasks.TextBoxGroupTask(
                new IBaseTask[3] {
                    new MyTasks.Acknowledge_TaskIsComplete(this),
                    new MyTasks.TextBoxAdjust_002(longLifeObjectManager.currentController.locker,longLifeObjectManager.textBox,20,0.1f),
                    new MyTasks.CameraMove_Zoom_001(longLifeObjectManager.currentController.locker,tarCam,1f,tarCam.gameObject.transform.position,10,0.7f,true)
                }
                ));
            
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
