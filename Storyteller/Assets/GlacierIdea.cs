using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlacierIdea : StorytellingIdea {

    [SerializeField]
    private GameObject glaciers;

    public override void ExecuteIdea() {
        glaciers.SetActive(true);
    }


}
