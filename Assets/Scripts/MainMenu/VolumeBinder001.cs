using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBinder001 : MonoBehaviour
{
    public GameProperties gameProperties;
    public string myType;
    public Text target;
    public Scrollbar source;

    private int float2Value(float f)
    {
        return (int)(f * 100);
    }

    private void Update()
    {
        target.text = float2Value(source.value).ToString();
        switch(myType)
        {
            case "audio":
                gameProperties.audioVolume = float2Value(source.value);
                break;
            case "effect":
                gameProperties.effectVolume = float2Value(source.value);
                break;
            default:
                break;
        }
    }
}
