using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyNamespace.mainMenuOption
{
    public class MainMenuOptions : MonoBehaviour
    {
        public Button[] buttons;
        public Text textOutput;
        private string[] strings = new string[] {
        "ѡ��",
        "��ѡ",
        "Option",
        "noitpO",
        };


        public void ShowString(int i)
        {
            textOutput.text = strings[i];
        }
    }
}
