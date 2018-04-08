using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspirationSource : MonoBehaviour {

    [SerializeField]
    private GameObject inspirationEffect;

    [SerializeField]
    private float inspirationStored = 10;

	private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GetComponent<ParticleSystem>().Stop();
            GameController.Instance.GainInspiration(inspirationStored);
            Instantiate(inspirationEffect, transform.position, Quaternion.Euler(Vector3.zero));
            //newInspirationEffect.transform.parent = other.transform;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

}
