using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interface;

namespace Interface
{
    public interface ITipBase
    {
        SpriteRenderer GetRenderer();
    }
}

public class TipCarrier001 : MonoBehaviour, ITipBase
{
    public SpriteRenderer GetRenderer()
    {
        return gameObject.GetComponent<SpriteRenderer>();
    }
}
