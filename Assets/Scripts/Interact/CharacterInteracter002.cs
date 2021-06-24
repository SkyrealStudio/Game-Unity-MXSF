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
    public GameObject tipObject;

    private List<long> tickID_list;

    public bool needFeed;

    public void InteractIsComplete()
    {
        needFeed = false;
    }

    private void Start()
    {
        tickID_list = new List<long>();
        tipObject.SetActive(true);
        needFeed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacterGObj) 
            && needFeed)
        {
            tickID_list.Add(longLifeObjectManager.tickRecorder.tickCount);
            //把一个接口实例放入玩家的接口结构中
            MainCharacterDominantor.MytaskAssemble001 tAssemble = 
                new MainCharacterDominantor.MytaskAssemble001(longLifeObjectManager.tickRecorder.tickCount);
            
            tAssemble.Enqueue(new MyTasks.CameraMove_Zoom_001(
                longLifeObjectManager.currentController.locker,
                tarCam,
                0.5f,
                tarCam.gameObject.transform.position + new Vector3(0f, -1f),
                10,
                0.7f
            ));
            
            tAssemble.Enqueue(new ConnecterTask(2));
            
            tAssemble.Enqueue(new MyTasks.TextBoxAdjust_001(
                longLifeObjectManager.currentController.locker,
                longLifeObjectManager.textBox,
                20,
                0.1f));//Open

            tAssemble.Enqueue(new MyTasks.TextBoxTextWork_001(
                longLifeObjectManager.textBox,
                "123456789012345678901234567890123456789012345678901234567890123456789-",
                40,
                0.2f,
                true
                ));

            tAssemble.Enqueue(new MyTasks.TextBoxTextWork_001(
                longLifeObjectManager.textBox,
                "12345678901234",
                40,
                0.2f,
                true
                ));

            tAssemble.Enqueue(new MyTasks.Acknowledge_TaskIsComplete(this));
            
            tAssemble.Enqueue(new ConnecterTask(2));

            tAssemble.Enqueue(new MyTasks.TextBoxAdjust_002(
                longLifeObjectManager.currentController.locker,
                longLifeObjectManager.textBox,
                20,
                0.1f));//Close

            tAssemble.Enqueue(new MyTasks.CameraMove_Zoom_001(
                longLifeObjectManager.currentController.locker,
                tarCam,
                1f,
                tarCam.gameObject.transform.position,
                10,
                0.7f,
                true
            ));
            targetDominantor.taskStack.Push(tAssemble);

            Debug.Log("in: "+gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacterGObj))
        {
            try
            {
                if (tickID_list.Contains(targetDominantor.taskStack.Tail().tickID))
                    targetDominantor.taskStack.Dequeue();
            }
            catch (System.Exception e)
            {
                
            }
            Debug.Log("leave: "+gameObject.name);
        }
    }

    
}
