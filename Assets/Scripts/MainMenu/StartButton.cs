using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.LoadSceneAsync("PlayField1");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
