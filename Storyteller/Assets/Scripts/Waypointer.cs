using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypointer : MonoBehaviour {

    private enum WaypointType {
        Loop,
        BackAndForth
    }

    [SerializeField]
    private WaypointType waypointType;

    [SerializeField]
    private List<Vector3> waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float bufferDistance;
    private float sqrBufferDistance;

    private bool isAscending = false;
    public bool IsAscending { get { return isAscending; } }

    private List<ObjectMovement> objectsOnPlatform;
	
    private void Awake() {
        transform.position = waypoints[currentWaypointIndex];
        sqrBufferDistance = Mathf.Pow(bufferDistance, 2);
        objectsOnPlatform = new List<ObjectMovement>();
    }

	private void FixedUpdate () {
        int nextWaypointIndex = currentWaypointIndex;
        if (waypointType == WaypointType.Loop) {
            nextWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        } else if (waypointType == WaypointType.BackAndForth) {
            nextWaypointIndex = currentWaypointIndex + 1;
            if (nextWaypointIndex >= waypoints.Count) {
                waypoints.Reverse();
            }
            nextWaypointIndex %= waypoints.Count;
        }
        Vector3 prevPosition = transform.position;
        transform.position = Vector3.Lerp(transform.position, waypoints[nextWaypointIndex], speed * Time.deltaTime);
        if ((transform.position - waypoints[nextWaypointIndex]).sqrMagnitude <= sqrBufferDistance) {
            currentWaypointIndex = nextWaypointIndex;
        }

        Vector3 deltaPosition = transform.position - prevPosition;
        for (int i = 0; i < objectsOnPlatform.Count; i++) {
            if (objectsOnPlatform[i].IsGrounded) {
                objectsOnPlatform[i].transform.Translate(deltaPosition);
            }
        }

        if (deltaPosition.y > 0) {
            isAscending = true;
        } else {
            isAscending = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        ObjectMovement otherObjectMovement = other.GetComponent<ObjectMovement>();
        if (otherObjectMovement) {
            objectsOnPlatform.Add(otherObjectMovement);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        ObjectMovement otherObjectMovement = other.GetComponent<ObjectMovement>();
        if (otherObjectMovement) {
            objectsOnPlatform.Remove(otherObjectMovement);
        }
    }

}
