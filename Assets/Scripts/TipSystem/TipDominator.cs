using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyNamespace;
using Assets.MyStructures;

public class TipDominator : MonoBehaviour
{
    public MainCharacterDominantor mainCharacterDominantor;

    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    public void Adjust()
    {
        foreach (SpriteRenderer iterator in spriteRenderers)
        {
            iterator.color = Color.white;
        }

        spriteRenderers = new List<SpriteRenderer>();
        for (int i = mainCharacterDominantor.taskStack.GetStartIndex; i < mainCharacterDominantor.taskStack.Count + mainCharacterDominantor.taskStack.GetStartIndex; i++)
        {
            spriteRenderers.Add(mainCharacterDominantor.taskStack.data[i].tipCarrier.GetRenderer());
        }

        if(mainCharacterDominantor.taskStack.Count!=0)
        {
            for (int i = mainCharacterDominantor.taskStack.GetStartIndex + mainCharacterDominantor.taskStack.Count - 1; i >= mainCharacterDominantor.taskStack.GetStartIndex; i--)
            {
                if (mainCharacterDominantor.taskStack.data[i].tipCarrier.GetRenderer().gameObject.activeSelf)
                {
                    mainCharacterDominantor.taskStack.data[i].tipCarrier.GetRenderer().color = Color.yellow;
                    break;
                }
                else
                {
                    continue;
                }
            }
            
        }
            //if(mainCharacterDominantor.taskStack.Top().tipCarrier.GetRenderer().gameObject.activeSelf)
            //    mainCharacterDominantor.taskStack.Top().tipCarrier.GetRenderer().color = Color.yellow;
            //else
            //    mainCharacterDominantor.taskStack.Top().tipCarrier.GetRenderer().color = Color.white;
    }
}
