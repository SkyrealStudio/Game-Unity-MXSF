using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyNamespace;



public class LongLifeObjectManager : MonoBehaviour,ITickRecorder
{
    
    public Controller001 currentController;
    
    public GameObject MainCharacterGObj;
    
    public GameProperties gameProperties;
    
    public TextBox textBox;
    //public TipDominator tipDominator;

    public TipTextBoxBranch tipTextBoxBranch;
    public CameraExecuter cameraExecuter;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        tickCount++;
    }
    private int tickCount = 0;
    public int GetTickCount() { return tickCount; }
}
