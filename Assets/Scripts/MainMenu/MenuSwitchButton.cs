using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSwitchButton : MonoBehaviour
{
    public GameObject target;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
            () => {
                gameObject.transform.parent.gameObject.SetActive(false);
                target.SetActive(true);
            }
        );
    }
}
