using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launchpad : MonoBehaviour {

    private enum LaunchType {
        OnDescend,
        OnAscend,
        Any
    }

    [SerializeField]
    private LaunchType launchType = LaunchType.OnDescend;

    [SerializeField]
    private Vector2 launchDirection;
    [SerializeField]
    private float launchSpeed;
    [SerializeField]
    private string animationTrigger;

    private Vector2 normalizedLaunchDirection;
    [SerializeField]
    private Animator launchpadAnimator;

    private void Awake() {
        normalizedLaunchDirection = launchDirection.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            ObjectMovement otherMovement = other.GetComponent<ObjectMovement>();
            if (launchType == LaunchType.OnDescend) {
                if (otherMovement.Velocity.y < 0) {
                    Launch(otherMovement);
                }
            } else if (launchType == LaunchType.OnAscend) {
                if (otherMovement.Velocity.y < 0) {
                    Launch(otherMovement);
                }
            } else if (launchType == LaunchType.Any) {
                Launch(otherMovement);
            }

        }
    }

    private void Launch(ObjectMovement launchedObjectMovement) {
        launchpadAnimator.SetTrigger(animationTrigger);
        launchedObjectMovement.Velocity = normalizedLaunchDirection * launchSpeed;
    }
}
