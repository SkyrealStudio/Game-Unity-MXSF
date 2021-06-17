using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickRecorder : MonoBehaviour
{
    internal long tickCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tickCount++;
    }
}
