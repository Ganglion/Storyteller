using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovementModifier : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Debug.Log("POG");
            other.GetComponent<ObjectMovement>().GravityMultiplier /= 3.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Debug.Log("Champ");
            other.GetComponent<ObjectMovement>().GravityMultiplier *= 3.5f;
        }
    }
}
