using UnityEngine;
using UnityEngine.UI;

public class CloseMenu : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    private void Awake()
    {

    }

    private void Start()
    {
        _button.onClick.AddListener(GameObject.Find("Menu").GetComponent<MenuBehavior>().hide);
    }
}