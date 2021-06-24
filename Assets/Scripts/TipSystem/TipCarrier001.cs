using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyNamespace;

namespace MyNamespace
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
