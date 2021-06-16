using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyNamespace;

public class LongLifeObjectManager : MonoBehaviour
{
    public Controller001 currentController;
    public GameObject MainCharacter;
    public Text outTextUI;
    //public Queue<string> PlotString;
    public GameProperties gameProperties;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        try
        {
            DontDestroyOnLoad(MainCharacter);
            DontDestroyOnLoad(gameProperties.gameObject);
        }
        catch (System.Exception e)
        {
            
        }
    }
}
