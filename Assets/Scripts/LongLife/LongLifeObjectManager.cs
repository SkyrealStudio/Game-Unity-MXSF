using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyNamespace;

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

    public TipTextBoxBranch tipTextBoxBranch;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
