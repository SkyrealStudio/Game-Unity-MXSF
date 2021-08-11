using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Shower
{
    public class TextBox : MonoBehaviour
    {
        public Image imageComponent_Main;
        public Text textComponent_Main;

        public Image[] imageComponents_Branch;
        public Text[] textComponents_Branch;
        public BranchOperation branchOperation;

        public class BranchOperation
        {
            private int _branchesCount;
            public int editPointer;
            private const int _branchesLimit = 3;
            public TextBox textBox;

            public int BranchesCount { get => _branchesCount; }

            public BranchOperation(TextBox textBox)
            {
                this.textBox = textBox;
                editPointer = 0;
                _branchesCount = 0;
            }

            public void AddBranch(string str)
            {
                if (_branchesCount == _branchesLimit)
                    throw new System.Exception("Too much branches requesting... out of range | BranchOperation.AddBranch");
                else
                {
                    textBox.textComponents_Branch[editPointer].text = str;
                    textBox.activatedBranchesCount = ++_branchesCount;
                    editPointer++;

                }
            }

            public void ClearBranchs()
            {
                editPointer = 0;
                _branchesCount = 0;
                for (int i = 0; i < _branchesLimit; i++)
                    textBox.textComponents_Branch[i].text = "[Cleared]";
                textBox.activatedBranchesCount = 0;
            }

            public async void ShowBranchs()
            {
                throw new System.NotImplementedException();
            }
            public async void HideBranchs()
            {
                throw new System.NotImplementedException();
            }
        }

        private void Awake()
        {
            branchOperation = new BranchOperation(this);
        }

        public int activatedBranchesCount = 0;

        public void ClearText_Main()
        {
            textComponent_Main.text = "";
        }

        public void AppendText_Main(string s)
        {
            textComponent_Main.text += s;
        }
        public void AppendText_Main(char c)
        {
            textComponent_Main.text += c;
        }

        public void SetText_Force_Main(string s)
        {
            textComponent_Main.text = s;
        }

        public void SetText_Force_Branches(string[] s_arr)
        {
            if (s_arr.Length > textComponents_Branch.Length)
                throw new System.Exception("Too much branches requesting... out of range | SetBranches");
            for (int i = 0; i < s_arr.Length; i++)
                textComponents_Branch[i].text = s_arr[i];
        }



    }
}