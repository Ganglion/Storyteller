using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorytellingIdea : MonoBehaviour {

    [SerializeField]
    private int ideaTier;
    public int IdeaTier { get { return ideaTier; } }

    [SerializeField]
    private string flavourText;
    public string FlavourText { get { return flavourText; } }

    public virtual void StartIdea() {
        gameObject.SetActive(true);
    }

    public virtual void ExecuteIdea() {
        
    }

    public virtual void EndIdea() {
        gameObject.SetActive(false);
    }

}
