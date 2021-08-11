using UnityEngine;

public class TestScript001 : MonoBehaviour
{
    private void Start()
    {
        AttributeManager.AttributeManager am = new AttributeManager.AttributeManager();
        print(am.Contains("wtc", "awake"));
    }
}