using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProperties : MonoBehaviour
{
    public Text droper;

    public string GamerName = "";
    public int audioVolume;
    public int effectVolume;


    [HideInInspector]
    public bool bloodSwitch = false;
    [HideInInspector]
    public float autoSavingGapSeconds = 600f;


    private void Update()
    {
        droper.text = "";
        droper.text += "Name:"+GamerName + "\n";
        droper.text += "AudioVolume:"+audioVolume + "\n";
        droper.text += "EffectVolume:"+effectVolume;
    }
}
