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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //try
        //{
        //    DontDestroyOnLoad(MainCharacterGObj);
        //    DontDestroyOnLoad(gameProperties.gameObject);
        //    DontDestroyOnLoad(tickRecorder.gameObject);
        //    DontDestroyOnLoad(textBox.gameObject);
        //}
        //catch (System.Exception e)
        //{
            
        //}
    }
}
