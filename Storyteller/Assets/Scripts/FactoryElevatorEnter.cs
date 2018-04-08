using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryElevatorEnter : MonoBehaviour {

    [SerializeField]
    private string storytellingBlockName;

    private bool inElevator = false;
    [SerializeField]
    private float elevatorShakeIntervalMin;
    [SerializeField]
    private float elevatorShakeIntervalMax;
    private float currentElevatorShakeInteral;

    private void Awake() {
        currentElevatorShakeInteral = Random.Range(elevatorShakeIntervalMin, elevatorShakeIntervalMax);
    }

    private void Update() {
        if (inElevator) {
            currentElevatorShakeInteral -= Time.deltaTime;

            if (currentElevatorShakeInteral <= 0) {
                ElevatorShake();
                currentElevatorShakeInteral += Random.Range(elevatorShakeIntervalMin, elevatorShakeIntervalMax);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            FlowchartController.Instance.ExecuteStorytellingBlock(storytellingBlockName);
            inElevator = true;
            //GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void ElevatorShake() {
        CameraController.Instance.ShakeCamera(Random.Range(0.003125f, 0.0625f), Random.Range(1f, 2.5f));
    }

    public void ExitElevator() {
        inElevator = false;
    }

}
