using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour {

    [Header("Collision")]
    [SerializeField]
    private int horizontalRaycastNumber;
    [SerializeField]
    private int verticalRaycastNumber;

    private Collider2D objectCollider;
    private LayerMask collisionLayerMask;

    private bool isGrounded = false;

    private const float BUFFER_LENGTH = 0.005f;
    private const string OBSTACLE_LAYER = "Obstacle";
    private const int INVALID_INDEX = -1;

    [Header("Movement")]

    private Vector2 velocity;

    private void Awake() {
        objectCollider = GetComponent<Collider2D>();
        collisionLayerMask = LayerMask.GetMask(OBSTACLE_LAYER);
    }

    private void Update() {
        velocity += new Vector2(0, -9.81f) * Time.deltaTime;
    }

    private void FixedUpdate() {
        if (isGrounded || velocity.y < 0) {
            ProcessVerticalMovement();
        }
    }

    private void ProcessHorizontalMovement() {

        Bounds objectColliderBounds = objectCollider.bounds;
        Vector2 startRaycastPoint = new Vector2(objectColliderBounds.center.x, objectColliderBounds.min.y);
        Vector2 endRaycastPoint = new Vector2(objectColliderBounds.center.x, objectColliderBounds.max.y);
        float raycastDistance = (objectColliderBounds.size.x / 2) + Mathf.Abs(velocity.x * Time.deltaTime);
        Vector2 raycastDirection = Mathf.Sign(velocity.x) > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D[] horizontalHits = new RaycastHit2D[horizontalRaycastNumber];
        
        for (int i = 0; i < horizontalRaycastNumber; i++) {
            float lerpAmount = i / (verticalRaycastNumber - 1);
            Vector2 raycastOrigin = Vector2.Lerp(startRaycastPoint, endRaycastPoint, lerpAmount);
            horizontalHits[i] = Physics2D.Raycast(raycastOrigin, raycastDirection, raycastDistance, collisionLayerMask);


        }

    }

    private void ProcessVerticalMovement() {

        Bounds objectColliderBounds = objectCollider.bounds;
        Vector2 startRaycastPoint = new Vector2(objectColliderBounds.min.x + BUFFER_LENGTH, objectColliderBounds.center.y);
        Vector2 endRaycastPoint = new Vector2(objectColliderBounds.max.x - BUFFER_LENGTH, objectColliderBounds.center.y);
        float raycastDistance = (objectColliderBounds.size.y / 2) + (isGrounded ? BUFFER_LENGTH : Mathf.Abs(velocity.y * Time.deltaTime));

        RaycastHit2D[] verticalHits = new RaycastHit2D[verticalRaycastNumber];
        float smallestRaycastFraction = Mathf.Infinity;
        int raycastIndexUsed = INVALID_INDEX;

        for (int i = 0; i < verticalRaycastNumber; i++) {
            float lerpAmount = i / (verticalRaycastNumber - 1);
            Vector2 raycastOrigin = Vector2.Lerp(startRaycastPoint, endRaycastPoint, lerpAmount);
            verticalHits[i] = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance, collisionLayerMask);

            if (verticalHits[i].fraction > 0 && verticalHits[i].fraction < smallestRaycastFraction) {
                raycastIndexUsed = i;
                smallestRaycastFraction = verticalHits[i].fraction;
            }
        }

        if (raycastIndexUsed != INVALID_INDEX) {
            isGrounded = true;
            float distanceToObstacle = verticalHits[raycastIndexUsed].fraction * raycastDistance - (objectColliderBounds.size.y / 2);
            transform.Translate(Vector3.down * distanceToObstacle);
            velocity = new Vector2(velocity.x, 0);
        } else {
            transform.Translate(new Vector3(0, velocity.y, 0) * Time.deltaTime);
            isGrounded = false;
        }
    }

}
