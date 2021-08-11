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
        private Queue<Unit_Mk004> _Units = new Queue<Unit_Mk004>(0);
        private Stack<Unit_Mk004> Units = new Stack<Unit_Mk004>(0);
        private void CrackString(string scriptString)
        {
            int endIndex;
            while ((endIndex = scriptString.IndexOf("//")) != -1)
                scriptString = scriptString.Remove(endIndex, scriptString.IndexOf('\n', endIndex + 1) - endIndex - 1);
            Queue<string> contents = new Queue<string>(0);
            Queue<int> indexs = new Queue<int>(0);
            for (int i = 0; i < scriptString.Length; i++)
            {
                switch (scriptString[i])
                {
                    // ${index}$
                    case '$':
                        endIndex = scriptString.IndexOf('$', i + 1);
                        if (Units.Count > 0)
                        {
                            Units.Peek().Contents = contents.ToArray();
                            Units.Peek().PossibleNextIndexs = indexs.ToArray();
                        }
                        Units.Push(new Unit_Mk004());
                        Units.Peek().SelfIndex = int.Parse(scriptString.Substring(i + 1, endIndex - i - 1));
                        Units.Peek().Style = TextStyle.EventTrigger;
                        contents = new Queue<string>(0);
                        indexs = new Queue<int>(0);
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
                        contents.Enqueue(scriptString.Substring(i + 1, endIndex - i - 1));
                        Units.Peek().Style = TextStyle.Plain;
                        i = endIndex + 1;
                        break;
                    // #plaingoto#
                    // #eventtrigger#
                    // #selective#goto#
                    case '#':
                        endIndex = scriptString.IndexOf('#', i + 1);
                        if (Units.Peek().Style == TextStyle.Plain)
                            indexs.Enqueue(int.Parse(scriptString.Substring(i + 1, endIndex - i - 1)));
                        else
                        {
                            contents.Enqueue(scriptString.Substring(i + 1, endIndex - i - 1));
                            if (scriptString.IndexOf('#', endIndex + 1) < scriptString.IndexOf('\n', endIndex + 1))
                            {
                                Units.Peek().Style = TextStyle.Selective;
                                i = endIndex;
                                endIndex = scriptString.IndexOf('#', endIndex + 1);
                                indexs.Enqueue(int.Parse(scriptString.Substring(i + 1, endIndex - i - 1)));
                            }
                        }
                        i = endIndex + 1;
                        break;
                    default:
                        break;
                }
            }
            int size = Units.Count;
            for(int i = 0; i < size; i++)
            {
                _Units.Enqueue(Units.Pop());
            }
            for (int i = 0; i < size; i++)
            {
                Units.Push(_Units.Dequeue());
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