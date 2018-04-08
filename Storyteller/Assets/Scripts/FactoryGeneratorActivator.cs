using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ferr;

public class FactoryGeneratorActivator : MonoBehaviour {

    [SerializeField]
    private GameObject unpoweredGenerator;
    [SerializeField]
    private GameObject poweredGenerator;
    [SerializeField]
    private string storytellingBlockName;
    [SerializeField]
    private FactoryElevatorActivation factoryElevator;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            ActivateGenerator();
        }
    }

    private void ActivateGenerator() {
        unpoweredGenerator.SetActive(false);
        poweredGenerator.SetActive(true);
        factoryElevator.activateAGenerator();
        GetComponent<BoxCollider2D>().enabled = false;
        if (storytellingBlockName != "") {
            FlowchartController.Instance.ExecuteStorytellingBlock(storytellingBlockName);
        }
    }
}
