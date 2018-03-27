using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launchpad : MonoBehaviour {

    [SerializeField]
    private Vector2 launchDirection;
    [SerializeField]
    private float launchSpeed;

    private Vector2 normalizedLaunchDirection;

    private void Awake() {
        normalizedLaunchDirection = launchDirection.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            ObjectMovement otherMovement = other.GetComponent<ObjectMovement>();
            if (otherMovement.Velocity.y < 0) {
                Vector2 otherVelocity = otherMovement.Velocity;
                otherVelocity.y = launchSpeed;
                otherMovement.Velocity = otherVelocity;
            }
        }
    }
}
