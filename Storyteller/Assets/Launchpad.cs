using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launchpad : MonoBehaviour {

    [SerializeField]
    private Vector2 launchDirection;
    [SerializeField]
    private float launchSpeed;
    [SerializeField]
    private string animationTrigger;

    private Vector2 normalizedLaunchDirection;
    private Animator launchpadAnimator;

    private void Awake() {
        normalizedLaunchDirection = launchDirection.normalized;
        launchpadAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            ObjectMovement otherMovement = other.GetComponent<ObjectMovement>();
            if (otherMovement.Velocity.y < 0) {
                launchpadAnimator.SetTrigger(animationTrigger);
                //Vector2 otherVelocity = otherMovement.Velocity;
                //otherVelocity.y = launchSpeed;
                //otherMovement.Velocity = otherVelocity;
                otherMovement.Velocity = normalizedLaunchDirection * launchSpeed;
            }
        }
    }
}
