using UnityEngine;

public class DialogUI : MonoBehaviour
{
    private bool _sentenceCompleted = true;

    [SerializeField]
    private PersistentObjectManager Manager;

    public void ForwardSentence()
    {
        if (this._sentenceCompleted)
        {
            this.NextSentence();
        }
        else
        {
            this.SkipSentence();
        }
    }

    private void NextSentence()
    {
        this._sentenceCompleted = false;
    }

    private void SkipSentence()
    {
        this._sentenceCompleted = true;
    }
}