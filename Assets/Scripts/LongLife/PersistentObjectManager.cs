using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Scripts;
using Scripts.persistentObject;

using Interface.Task;
using Interface.Tick;
using Translator;
using Shower;
using Scripts.Task.Chain;
using Interface.Task.Chain;

public class PersistentObjectManager : MonoBehaviour,ITickRecorder
{
    public Controller001 currentController;
    
    public GameObject MainCharacterGObj;
    
    public GameProperties gameProperties;

    public IParserUnitToTaskInterface parserTranslator;
    public IParserUnitModifier parserUnitModifier;
    
    //public TextBox textBox;
    //public TipDominator tipDominator;

    public TipTextBoxBranch tipTextBoxBranch;
    public CameraExecuter cameraExecuter;

    public DefaultUIShowerSetting DefaultUIShowerSetting;
    
    private void Awake()
    {
        parserUnitModifier = new ChainUnitModifier();
        parserTranslator = new ParserUnitToTaskTranslator(this);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        tickCount++;
    }
    private int tickCount = 0;
    public int GetTickCount() { return tickCount; }
}

namespace Scripts.persistentObject
{
    public class DefaultUIShowerSetting
    {
        public DefaultUIShowerSetting(TextBox UItextBox, float speedPerChar)
        {
            this.UItextBox = UItextBox;
            this.speedPerChar = speedPerChar;
        }
        public TextBox UItextBox;
        public float speedPerChar;
    }
}