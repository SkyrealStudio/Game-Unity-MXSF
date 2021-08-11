using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Interface.TextParser;
using Interface.TextParser.ReturnUnit;

namespace BackUpSpace.ScriptReader
{
    /*20210810
     *from @TianC-Wang
     *反省一下--
     *似乎TNT心态有点炸--ycMia应该下次说话注意分寸
     */

    /// <summary>Plot script input.</summary>
    class ScriptReader : ITextParser_Mk004
    {
        /// <summary>Target file stream.</summary>
        private FileStream ScriptFile;
        
        /// <summary>Preview the next byte.</summary>
        private char PreviewByte()
        {
            char ret = (char)ScriptFile.ReadByte();
            ScriptFile.Seek(-1, SeekOrigin.Current);
            return ret;
        }

        /// <summary>Preview a slice of bytes.</summary>
        private string PreviewLength(int length)
        {
            string ret = "";
            for (int i = 0; i < length; i++)
                ret += (char)ScriptFile.ReadByte();
            ScriptFile.Seek(-length, SeekOrigin.Current);
            return ret;
        }

        /// <summary>Read until it found the target byte, won't include or skip the target byte.</summary>
        private string ReadUntil(char chr)
        {
            string ret = "";
            while (PreviewByte() != chr && ScriptFile.Position != ScriptFile.Length - 1)
            {
                if (PreviewLength(2) == "//")
                {
                    ScriptFile.ReadByte();
                    ReadLine();
                }
                ret += (char)ScriptFile.ReadByte();
            }
            return ret;
        }

        /// <summary>Read until it found one of the target bytes, won't include or skip the target byte.</summary>
        private string ReadUntil(char[] chrs)
        {
            string ret = "";
            while (ScriptFile.Position != ScriptFile.Length - 1)
            {
                foreach (char chr in chrs)
                    if (chr == PreviewByte())
                        return ret;
                if (PreviewLength(2) == "//")
                {
                    ScriptFile.ReadByte();
                    ReadLine();
                }
                ret += (char)ScriptFile.ReadByte();
            }
            return ret;
        }

        /// <summary>Read until the target string pattern, won't include or skip the target string.</summary>
        private string ReadUntil(string str)
        {
            if (str.Length == 0)
                return "";
            string ret = "";
            ret += ReadUntil(str[0]);
            while (str != PreviewLength(str.Length) && ScriptFile.Position != ScriptFile.Length - 1)
            {
                ret += (char)ScriptFile.ReadByte();
                ret += ReadUntil(str[0]);
            }
            return ret;
        }

        /// <summary>Read the bytes between a signed area, won't include but skip the start/end signs.</summary>
        private string ReadBetween(string startSign, string endSign)
        {
            ReadUntil(startSign);
            ScriptFile.Seek(startSign.Length, SeekOrigin.Current);
            string ret = ReadUntil(endSign);
            ScriptFile.Seek(endSign.Length, SeekOrigin.Current);
            return ret;
        }

        /// <summary>Read a line of bytes, won't include but skip the line end.</summary>
        private string ReadLine()
        {
            string ret = "";
            ret = ReadUntil(new char[] { '\n', '\r' });
            ScriptFile.ReadByte();
            return ret;
        }

        /// <summary>Read a sequence of not-blank bytes.</summary>
        private string ReadWord()
        {
            return ReadUntil(new char[] { ' ', '\n', '\r', '\t' });
        }

        /// <summary>Generate an instance of ScriptReader class for scripts' reading.</summary>
        public ScriptReader(string scriptFilePath)
        {
            SetTargetFile(scriptFilePath);
        }

        /// <summary>Set the target file of this instance to another one.</summary>
        public bool SetTargetFile(string scriptFilePath)
        {
            try { ScriptFile.Close(); }
            catch (Exception) { }
            try
            {
                ScriptFile = new FileStream(Application.streamingAssetsPath + "\\" + scriptFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>Get a node of plot.</summary>
        public Unit_Mk004 GetUnit(ulong index)
        {
            ulong plainNext = 0;
            List<string> contents = new List<string>(0);
            List<ulong> indexs = new List<ulong>(0);
            Unit_Mk004 ret = new Unit_Mk004();
            ret.SelfIndex = index;
            ScriptFile.Seek(0, SeekOrigin.Begin);
            while (ReadBetween("$", "$") != index.ToString()) ;
            ret.Speaker = ReadBetween("[", "]");
            ret.Style = TextStyle.eventTrigger;
            while (true)
            {
                ReadUntil(new char[] { ':', '#', '$' });
                switch ((char)ScriptFile.ReadByte())
                {
                    case '#':
                        if (ret.Style != TextStyle.plain)
                        {
                            contents.Add(ReadUntil(new char[] { '\n', '\r', '#' }));
                            switch ((char)ScriptFile.ReadByte())
                            {
                                case '#':
                                    ret.Style = TextStyle.selective;
                                    indexs.Add(ulong.Parse(ReadWord()));
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            plainNext = ulong.Parse(ReadLine());
                        break;
                    case ':':
                        ret.Style = TextStyle.plain;
                        contents.Add(ReadLine());
                        break;
                    case '$':
                    default:
                        ret.Contents = contents.ToArray();
                        ret.PossibleNextIndexs = indexs.Count == 0 ? new ulong[] { plainNext } : indexs.ToArray();
                        ScriptFile.Seek(-1, SeekOrigin.Current);
                        return ret;
                }
            }
        }
    }
}