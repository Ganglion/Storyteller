using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour {

    [SerializeField]
    private float climbSpeed;
    [SerializeField]
    private float climbAcceleration;

    private ObjectMovement objectClimbing;

	private void Awake() {
    }

    private void FixedUpdate() {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            other.GetComponent<ObjectMovement>().StartFloating(climbSpeed, climbAcceleration);
            
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            other.GetComponent<ObjectMovement>().StopFloating();
        }
    }
}
