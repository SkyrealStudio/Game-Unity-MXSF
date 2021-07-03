using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyNamespace;

public class ChooseForm
{
    public ChooseForm()
    {
        _data = new string[1];
        _current = 0;
    }
    public void Choose(string value)
    {
        if(_current==_data.Length)
        {
            string[] trans = new string[_data.Length * 2];
            for(int i =0;i<_data.Length;i++)
            {
                trans[i] = _data[i];
            }
            _data = trans;
        }
        _data[_current] = value;
    }
    
    public int NowStepCount { get => _current; }
    public string LastRecord
    {
        get
        {
            return (_current - 1 >=0) ? _data[_current-1] : "NULL";
        }
    }

    public bool Includes(string comp)
    {
        foreach (string i in _data)
            if (i == comp) return true;
        return false;
    }
    
    public string Debug_StepRecord
    {
        get
        {
            string rev = "";
            for (int i = 0; i < _data.Length; i++)
                rev += (_data[i] + '\n');
            return rev;
        }

    }

    private string[] _data;
    private int _current;
}

public class LongLifeObjectManager : MonoBehaviour
{
    public Controller001 currentController;
    public GameObject MainCharacterGObj;
    public Text outTextUI;
    //public Queue<string> PlotString;
    public GameProperties gameProperties;
    public TickRecorder tickRecorder;
    public TextBox textBox;
    public TipDominator tipDominator;
    public ChooseForm chooseForm = new ChooseForm();

    public TipTextBoxBranch tipTextBoxBranch;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
