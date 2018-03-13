using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowersIdea : StorytellingIdea {

    [SerializeField]
    private GameObject flowers;

    public override void ExecuteIdea() {
        flowers.SetActive(true);
    }

}
