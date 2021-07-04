using Unity;
using UnityEngine;

public class ChoiceForm
{
    public ChoiceForm()
    {
        _data = new string[1];
        _currentPointer = 0;
    }
    public void Choose(string value)
    {
        if (_currentPointer == _data.Length)
        {
            string[] trans = new string[_data.Length * 2];
            for (int i = 0; i < _data.Length; i++)
            {
                trans[i] = _data[i];
            }
            _data = trans;
        }
        _data[_currentPointer] = value;
        _currentPointer++;
    }

    public int NowStepCount { get => _currentPointer; }
    public string LastRecord
    {
        get
        {
            return (_currentPointer - 1 >= 0) ? _data[_currentPointer - 1] : "NULL";
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
    private int _currentPointer;
}

public class ChoiceFormCarrier : MonoBehaviour
{
    public ChoiceForm choiceForm;
    private void Awake()
    {
        choiceForm = new ChoiceForm();
    }
}