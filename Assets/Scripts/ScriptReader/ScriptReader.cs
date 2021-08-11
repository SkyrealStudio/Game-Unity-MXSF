using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Interface.TextParser;
using Interface.TextParser.ReturnUnit;

namespace ScriptReader
{
    /// <summary>Plot script input.</summary>
    class ScriptReader : ITextParser_Mk004
    {
        private Stack<Unit_Mk004> Units = new Stack<Unit_Mk004>(0);
        private void CrackString(string scriptString)
        {
            int endIndex;
            while ((endIndex = scriptString.IndexOf("//")) != -1)
                scriptString = scriptString.Remove(endIndex, scriptString.IndexOf('\n', endIndex + 1) - endIndex - 1);
            Stack<string> contents = new Stack<string>(0);
            Stack<int> indexs = new Stack<int>(0);
            for (int i = 0; i < scriptString.Length; i++)
            {
                switch (scriptString[i])
                {
                    // ${index}$
                    case '$':
                        endIndex = scriptString.IndexOf('$', i + 1);
                        Units.Push(new Unit_Mk004());
                        Units.Peek().SelfIndex = int.Parse(scriptString.Substring(i + 1, endIndex - i - 1));
                        Units.Peek().Style = TextStyle.EventTrigger;
                        Units.Peek().Contents = contents.ToArray();
                        Units.Peek().PossibleNextIndexs = indexs.ToArray();
                        contents = new Stack<string>(0);
                        indexs = new Stack<int>(0);
                        i = endIndex + 1;
                        break;
                    // [{speaker}]
                    case '[':
                        endIndex = scriptString.IndexOf(']', i + 1);
                        Units.Peek().Speaker = scriptString.Substring(i + 1, endIndex - i - 1);
                        i = endIndex + 1;
                        break;
                    // :plaintext
                    case ':':
                        endIndex = scriptString.IndexOf("\r\n", i + 1);
                        contents.Push(scriptString.Substring(i + 1, endIndex - i - 1));
                        Units.Peek().Style = TextStyle.Plain;
                        i = endIndex + 1;
                        break;
                    // #plaingoto#
                    // #eventtrigger#
                    // #selective#goto#
                    case '#':
                        endIndex = scriptString.IndexOf('#', i + 1);
                        if (Units.Peek().Style == TextStyle.Plain)
                            indexs.Push(int.Parse(scriptString.Substring(i + 1, endIndex - i - 1)));
                        else
                        {
                            contents.Push(scriptString.Substring(i + 1, endIndex - i - 1));
                            if (scriptString.IndexOf('#', endIndex + 1) < scriptString.IndexOf('\n', endIndex + 1))
                            {
                                Units.Peek().Style = TextStyle.Selective;
                                i = endIndex;
                                endIndex = scriptString.IndexOf('#', endIndex + 1);
                                indexs.Push(int.Parse(scriptString.Substring(i + 1, endIndex - i - 1)));
                            }
                        }
                        i = endIndex + 1;
                        break;
                    default:
                        break;
                }
            }
        }
        public bool SetTargetFile(string scriptFilePath)
        {
            try
            {
                Units.Clear();
                CrackString(File.ReadAllText(Application.streamingAssetsPath + "\\" + scriptFilePath, Encoding.UTF8));
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                return false;
            }
        }
        public Unit_Mk004 GetUnit(int index)
        {
            foreach (Unit_Mk004 unit in Units)
                if (unit.SelfIndex == index)
                    return unit;
            throw new Exception("Index not found!");
        }
        public Unit_Mk004[] GetUnits()
        {
            return Units.ToArray();
        }
    }
}