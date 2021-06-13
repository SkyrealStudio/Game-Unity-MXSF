using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CharacterInteracter001 : MonoBehaviour
{
    public LongLifeObjectManager longLifeObjectManager;

    public MainCharacterDominantor targetDominantor;

    private class MyTextTask : ITextTask
    {
        public MyTextTask(string s)
        {
            _data = s;
        }

        public void Restore()
        {
            _outTextUI.text = "--";
        }

        public MyTextTask(string s, Text text)
        {
            BindTextUI(text);
            _data = s;
        }
        public void BindTextUI(Text inTextUI)
        {
            _outTextUI = inTextUI;
        }
        public void Execute()
        {
            if (_outTextUI)
                _outTextUI.text = _data;
            else
                throw new System.Exception("No upstream TextUI");
        }

        public string GetString()
        {
            return _data;
        }
        public TaskType GetTaskType()
        {
            return TaskType.StringType;
        }

        private string _data;
        public Text _outTextUI;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacter))
        {
            //把一个接口实例放入玩家的接口结构中
            targetDominantor.generalTaskStack.Push(new MyTextTask("阿巴阿巴",longLifeObjectManager.outTextUI));
            Debug.Log("in!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.Equals(longLifeObjectManager.MainCharacter))
        {
            targetDominantor.generalTaskStack.Peek().Restore();
            targetDominantor.generalTaskStack.Pop();
            Debug.Log("leave!");
        }
    }
}
