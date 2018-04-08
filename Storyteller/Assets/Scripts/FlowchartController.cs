using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FlowchartController : Singleton<FlowchartController> {

    private Flowchart flowchart;

    private void Awake() {
        flowchart = GetComponent<Flowchart>();
    }

    public void ExecuteStorytellingBlock(string blockName) {
        flowchart.StopAllBlocks();
        flowchart.ExecuteBlock(blockName);
    }

}
