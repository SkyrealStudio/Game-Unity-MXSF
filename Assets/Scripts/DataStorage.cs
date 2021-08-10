using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataStorage
{
    private string FileName;
    private IDictionary<string, string> Datas;
    public DataStorage(string FileName)
    {
        this.FileName = Application.persistentDataPath + FileName;
    }
    public DataStorage(string FileName, IDictionary<string, string> Datas)
    {
        this.FileName = Application.persistentDataPath + FileName;
        this.Datas = Datas;
    }
    private void ReadFileDatas()
    {
    }
}