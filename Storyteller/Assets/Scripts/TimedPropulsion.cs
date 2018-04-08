using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPropulsion : MonoBehaviour {

    [SerializeField]
    private ParticleSystem propulsionEffect;
    [SerializeField]
    private ParticleSystem warningEffect;

    [SerializeField]
    private float propulsionCooldown;
    [SerializeField]
    private Vector2 propulsionVelocity;
    [SerializeField]
    private float initialDelay;
    [SerializeField]
    private float warningDuration;
    private bool isWarning = false;
    private Vector2 normalisedPropulsionDirection;
    private float currentPropulsionCooldown;

    [SerializeField]
    private float shakeIntensity;
    [SerializeField]
    private float shakeDuration;

    private List<ObjectMovement> objectsInPropulsionArea;

    private void Awake() {
        if (propulsionEffect == null) {
            propulsionEffect = GetComponent<ParticleSystem>();
        }
        objectsInPropulsionArea = new List<ObjectMovement>();
        currentPropulsionCooldown = propulsionCooldown;

        ParticleSystem.EmissionModule warningEmission = warningEffect.emission;
        warningEmission.enabled = false;
        ParticleSystem[] warningEmissionChildren = warningEffect.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < warningEmissionChildren.Length; i++) {
            ParticleSystem.EmissionModule warningChildEmission = warningEmissionChildren[i].emission;
            warningChildEmission.enabled = false;
        }
    }

    private void Update() {

        if (initialDelay > 0) {
            initialDelay -= Time.deltaTime;
            return;
        }

        if (currentPropulsionCooldown > 0) {
            currentPropulsionCooldown -= Time.deltaTime;
            if (currentPropulsionCooldown <= warningDuration && !isWarning) {
                isWarning = true;
                ParticleSystem.EmissionModule warningEmission = warningEffect.emission;
                warningEmission.enabled = true;
                ParticleSystem[] warningEmissionChildren = warningEffect.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < warningEmissionChildren.Length; i++) {
                    ParticleSystem.EmissionModule warningChildEmission = warningEmissionChildren[i].emission;
                    warningChildEmission.enabled = true;
                }
            }
        } else {
            propulsionEffect.Play();
            ParticleSystem.EmissionModule warningEmission = warningEffect.emission;
            warningEmission.enabled = false;
            ParticleSystem[] warningEmissionChildren = warningEffect.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < warningEmissionChildren.Length; i++) {
                ParticleSystem.EmissionModule warningChildEmission = warningEmissionChildren[i].emission;
                warningChildEmission.enabled = false;
            }
            isWarning = false;
            Launch();
            currentPropulsionCooldown += propulsionCooldown;
        }

    }

    private void Launch() {
        if (objectsInPropulsionArea.Count > 0) {
            CameraController.Instance.ShakeCamera(shakeIntensity, shakeDuration);
        }
        for (int i = 0; i < objectsInPropulsionArea.Count; i++) {
            objectsInPropulsionArea[i].Velocity += propulsionVelocity;
            objectsInPropulsionArea[i].IsGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ObjectMovement otherObjectMovement = other.GetComponent<ObjectMovement>();
        if (otherObjectMovement) {
            objectsInPropulsionArea.Add(otherObjectMovement);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        ObjectMovement otherObjectMovement = other.GetComponent<ObjectMovement>();
        if (otherObjectMovement) {
            objectsInPropulsionArea.Remove(otherObjectMovement);
        }
    }


}
