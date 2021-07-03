using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public Image imageComponent_Main;
    public Text textComponent_Main;

    public Image[] imageComponents_Branch;
    public Text[] textComponents_Branch;

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
            throw new System.Exception("Too much Branches... out of range | SetBranches");
        for (int i = 0; i < s_arr.Length; i++)
            textComponents_Branch[i].text = s_arr[i];
    }
}
