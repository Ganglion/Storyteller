using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovementModifier : MonoBehaviour {

    [SerializeField]
    private float gravityMultiplierScale = 3.5f;

	private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            other.GetComponent<ObjectMovement>().GravityMultiplier /= gravityMultiplierScale;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            other.GetComponent<ObjectMovement>().GravityMultiplier *= gravityMultiplierScale;
        }
    }
}
