using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Interface.Attribute;

namespace AttributeManager
{
    public class AttributeManager : IAttributeInventory
    {
        private class JsonFormat
        {
            public string[] JsonObjects;
        }
        private class JsonObject
        {
            public string Character;
            public string[] Attributes;
        }
        private Dictionary<string, List<string>> AttributeDatas = new Dictionary<string, List<string>>(0);
        private void Save()
        {
            JsonFormat format = new JsonFormat();
            List<string> objs = new List<string>(0);
            foreach (KeyValuePair<string, List<string>> attrData in AttributeDatas)
            {
                JsonObject jo = new JsonObject();
                jo.Character = attrData.Key;
                jo.Attributes = attrData.Value.ToArray();
                objs.Add(JsonUtility.ToJson(jo));
            }
            format.JsonObjects = objs.ToArray();
            File.WriteAllText(Application.persistentDataPath + "\\Plot data.json", JsonUtility.ToJson(format), Encoding.UTF8);
        }
        private void Load()
        {
            try
            {
                AttributeDatas.Clear();
                JsonFormat format = JsonUtility.FromJson<JsonFormat>(File.ReadAllText(Application.persistentDataPath + "\\Plot data.json", Encoding.UTF8));
                foreach (string obj in format.JsonObjects)
                {
                    JsonObject jo = JsonUtility.FromJson<JsonObject>(obj);
                    AttributeDatas.Add(jo.Character, new List<string>(jo.Attributes));
                }
            }
            catch (Exception) { }
        }
        public AttributeManager() { Load(); }
        public bool AddAttribute(string tarChar, string attr)
        {
            if (!AttributeDatas.ContainsKey(tarChar))
            {
                List<string> attrs = new List<string>(0);
                attrs.Add(attr);
                AttributeDatas.Add(tarChar, attrs);
                Save();
                return false;
            }
            AttributeDatas[tarChar].Add(attr);
            Save();
            return true;
        }
        public bool RemoveAttribute(string tarChar, string attr)
        {
            if (!AttributeDatas.ContainsKey(tarChar))
                return false;
            if (!AttributeDatas[tarChar].Contains(attr))
                return false;
            AttributeDatas[tarChar].Remove(attr);
            Save();
            return true;
        }
        public bool Contains(string tarChar, string attr)
        {
            try { return AttributeDatas[tarChar].Contains(attr); }
            catch (Exception) { return false; }
        }
        public string[] ListAttribute(string tarChar)
        {
            try { return AttributeDatas[tarChar].ToArray(); }
            catch (Exception) { return new string[0]; }
        }
        public string[] SearchAttribute(string attr)
        {
            List<string> characters = new List<string>(0);
            foreach (KeyValuePair<string, List<string>> pair in AttributeDatas)
                if (pair.Value.Contains(attr))
                    characters.Add(pair.Key);
            return characters.ToArray();
        }
    }
}