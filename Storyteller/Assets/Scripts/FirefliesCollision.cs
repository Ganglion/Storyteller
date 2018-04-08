using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefliesCollision : MonoBehaviour {

    [SerializeField]
    private float inspirationPerFirefly = 5;

    [SerializeField]
    private GameObject inspirationEffect;

    private void OnParticleCollision(GameObject other) {
        Instantiate(inspirationEffect, other.transform.position, Quaternion.Euler(Vector3.zero));
        GameController.Instance.GainInspiration(inspirationPerFirefly);
    }

}
