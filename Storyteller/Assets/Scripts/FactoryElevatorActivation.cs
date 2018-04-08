using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryElevatorActivation : MonoBehaviour {

    [SerializeField]
    private GameObject unactivatedElevator;
    [SerializeField]
    private GameObject activatedElevator;
    [SerializeField]
    private GameObject unpressedButton;
    [SerializeField]
    private GameObject pressedButton;
    [SerializeField]
    private List<GameObject> redBars;
    [SerializeField]
    private List<GameObject> greenBars;
    [SerializeField]
    private ParticleSystem sparksPS;

    private int numberOfActivatedGenerators = 0;
	
	public void activateAGenerator() {
        numberOfActivatedGenerators++;
        redBars[numberOfActivatedGenerators - 1].SetActive(false);
        greenBars[numberOfActivatedGenerators - 1].SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (numberOfActivatedGenerators >= 4) {
                unactivatedElevator.SetActive(false);
                activatedElevator.SetActive(true);
                GetComponent<BoxCollider2D>().enabled = false;
            }
            unpressedButton.SetActive(false);
            pressedButton.SetActive(true);
            sparksPS.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            pressedButton.SetActive(false);
            unpressedButton.SetActive(true);
        }
    }

}
