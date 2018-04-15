using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSetter : MonoBehaviour {

    [SerializeField]
    private GameObject objToSetTag;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            SetBoundary(objToSetTag);
        }
    }

    public void SetBoundary(GameObject obj) {
        obj.tag = "Boundary";
    }

}
