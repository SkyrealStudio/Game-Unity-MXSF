using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxInit : MonoBehaviour
{
    TextBox textBox;

    private void Awake()
    {
        textBox = gameObject.GetComponent<TextBox>();

        Text textComponent_Main = textBox.textComponent_Main;
        Image imageComponent_Main = textBox.imageComponent_Main;

        Text[] textComponents_Branch = textBox.textComponents_Branch;
        Image[] imageComponents_Branch = textBox.imageComponents_Branch;

        imageComponent_Main.color = new Color(imageComponent_Main.color.r, imageComponent_Main.color.g, imageComponent_Main.color.b, 0f);
        textComponent_Main.color = new Color(textComponent_Main.color.r, textComponent_Main.color.g, textComponent_Main.color.b, 0f);

        foreach (Image i in imageComponents_Branch)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0f);
        }
        foreach (Text i in textComponents_Branch)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0f);
        }

        Destroy(this);
    }
}
