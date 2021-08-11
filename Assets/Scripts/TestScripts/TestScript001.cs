using UnityEngine;

public class TestScript001 : MonoBehaviour
{
    private void Start()
    {
        ScriptReader.ScriptReader sr = new ScriptReader.ScriptReader();
        sr.SetTargetFile("Test.txt");
    }
}