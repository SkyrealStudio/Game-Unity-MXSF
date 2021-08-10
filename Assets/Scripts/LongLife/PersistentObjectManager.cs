using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Scripts;

using Interface.Task;
using Interface.Tick;
using Translator;

public class PersistentObjectManager : MonoBehaviour,ITickRecorder
{
    
    public Controller001 currentController;
    
    public GameObject MainCharacterGObj;
    
    public GameProperties gameProperties;

    public ParserUnitToTaskInterface paserTranslator;

    public TextBox textBox;
    //public TipDominator tipDominator;

    public TipTextBoxBranch tipTextBoxBranch;
    public CameraExecuter cameraExecuter;

    private void Awake()
    {
        paserTranslator = new ParserUnitToTaskTranslator();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        tickCount++;
    }
    private int tickCount = 0;
    public int GetTickCount() { return tickCount; }
}
