using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipTextBoxBranch : MonoBehaviour
{
    public Image[] images;

    public void LightUP(int index)
    {
        images[index].color = Color.yellow;
    }
}
