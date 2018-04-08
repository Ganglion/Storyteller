using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryActivatable : MonoBehaviour {

    [SerializeField]
    private List<Waypointer> activatableWaypointers;

	private void Start() {
        for (int i = 0; i < activatableWaypointers.Count; i++) {
            activatableWaypointers[i].enabled = true;
        }
    }

}
