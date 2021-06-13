using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldBinder001 : MonoBehaviour
{
    public InputField source;
    public Button acceptButton;
    public GameProperties target_properties;

    public void _bindTarget2Properties()
    {
        target_properties.GamerName = source.text;
    }

    private void Start()
    {
        acceptButton.onClick.AddListener(_bindTarget2Properties);
    }
}
