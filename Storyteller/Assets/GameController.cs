using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {

    [Header("Resources")]
    private float inspiration = 150;
    private float imagination = 0;
    private float focus = 0;
    [SerializeField]
    private float baseConversionSpeed;
    [SerializeField]
    private float maxConversionSpeed;
    [SerializeField]
    private float timeToReachMaxConversionSpeed;
    private float currentConversionTime = 0;
    [SerializeField]
    private float inspirationDecrementRate = 0.5f;

    [SerializeField]
    private float inspirationGainSpeed;
    private float targetInspiration = 0;
    private bool isIncrementingInspiration = false;

    [SerializeField]
    private float baseShakeIntensity;
    [SerializeField]
    private float maxShakeIntensity;

    [SerializeField]
    private float focusUseRate;
    [SerializeField]
    private float focusRestoreRate;
    [SerializeField]
    private float focusMinUseValue;
    private bool isFocusing = false;

    [SerializeField]
    private Slider inspirationSlider;
    [SerializeField]
    private Slider imaginationSlider;
    [SerializeField]
    private Slider focusSlider;
    [SerializeField]
    private ParticleSystem imaginationSuperPS;

    [Header("Progress Tree")]
    [SerializeField]
    private StorytellingTree storytellingTree;

    [Header("Storyteller")]
    [SerializeField]
    private ObjectMovement storytellerMovement;
    [SerializeField]
    private ParticleSystem storytellerImaginingPS;
    [SerializeField]
    private ParticleSystem storytellerFocusPS;

    private void Awake() {
        focus = inspiration;

        ParticleSystem.EmissionModule imaginingEmission = storytellerImaginingPS.emission;
        imaginingEmission.enabled = false;
        ParticleSystem[] imaginingEmissionChildren = storytellerImaginingPS.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < imaginingEmissionChildren.Length; i++) {
            ParticleSystem.EmissionModule imaginingChildEmission = imaginingEmissionChildren[i].emission;
            imaginingChildEmission.enabled = false;
        }

        ParticleSystem.EmissionModule imaginationSuperEmission = imaginationSuperPS.emission;
        imaginationSuperEmission.enabled = false;

        ParticleSystem.EmissionModule focusLeapEmission = storytellerFocusPS.emission;
        focusLeapEmission.enabled = false;
    }

    private void Update () {

        if (storytellingTree.IsOpen) {
            return;
        }

        if (isIncrementingInspiration) {
            inspiration = Mathf.MoveTowards(inspiration, targetInspiration, inspirationGainSpeed * Time.deltaTime);
            if (inspiration == targetInspiration) {
                isIncrementingInspiration = false;
            }
        }

        inspiration = Mathf.MoveTowards(inspiration, 0, inspirationDecrementRate * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space) && inspiration > 0) {
            currentConversionTime += Time.deltaTime;
            float percentCharge = Mathf.Clamp01(currentConversionTime / timeToReachMaxConversionSpeed);
            float currentConversionSpeed = baseConversionSpeed + (maxConversionSpeed - baseConversionSpeed) * percentCharge;
            float currentShakeIntensity = baseShakeIntensity + (maxShakeIntensity - baseShakeIntensity) * percentCharge;
            CameraController.Instance.ShakeCamera(currentShakeIntensity, 0.05f);
            float conversionAmount = Mathf.Min(currentConversionSpeed * Time.deltaTime, inspiration);
            inspiration -= conversionAmount;
            imagination += conversionAmount;
            
            /*
            if (!storytellerImaginingPS.isEmitting) {
                ParticleSystem.EmissionModule imaginingEmission = storytellerImaginingPS.emission;
                imaginingEmission.enabled = true;
            }
            
            Debug.Log("BAR");
            if (!imaginationSuperPS.is) {
                ParticleSystem.EmissionModule imaginationSuperEmission = imaginationSuperPS.emission;
                Debug.Log("FOO");
                imaginationSuperEmission.enabled = true;
            }*/
            //if (storytellerImaginingPS.isStopped) {
                //storytellerImaginingPS.Play();
            //}

        } else {
            currentConversionTime = 0;

            /*
            if (storytellerImaginingPS.isEmitting) {
                ParticleSystem.EmissionModule imaginingEmission = storytellerImaginingPS.emission;
                imaginingEmission.enabled = false;
            }
            if (imaginationSuperPS.isEmitting) {
                ParticleSystem.EmissionModule imaginationSuperEmission = imaginationSuperPS.emission;
                imaginationSuperEmission.enabled = false;
            }*/
            //if (storytellerImaginingPS.isPlaying) {
                //storytellerImaginingPS.Stop();
            //}
        }

        if (Input.GetKeyDown(KeyCode.Space)) {

            ParticleSystem.EmissionModule imaginingEmission = storytellerImaginingPS.emission;
            imaginingEmission.enabled = true;
            ParticleSystem[] imaginingEmissionChildren = storytellerImaginingPS.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < imaginingEmissionChildren.Length; i++) {
                ParticleSystem.EmissionModule imaginingChildEmission = imaginingEmissionChildren[i].emission;
                imaginingChildEmission.enabled = true;
            }

            ParticleSystem.EmissionModule imaginationSuperEmission = imaginationSuperPS.emission;
            imaginationSuperEmission.enabled = true;

        } else if (Input.GetKeyUp(KeyCode.Space)) {

            ParticleSystem.EmissionModule imaginingEmission = storytellerImaginingPS.emission;
            imaginingEmission.enabled = false;
            ParticleSystem[] imaginingEmissionChildren = storytellerImaginingPS.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < imaginingEmissionChildren.Length; i++) {
                ParticleSystem.EmissionModule imaginingChildEmission = imaginingEmissionChildren[i].emission;
                imaginingChildEmission.enabled = false;
            }

            ParticleSystem.EmissionModule imaginationSuperEmission = imaginationSuperPS.emission;
            imaginationSuperEmission.enabled = true;

        }


        float braveryToUse = focusUseRate * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && focus >= braveryToUse) {
            isFocusing = true;
            storytellerMovement.GravityMultiplier /= 2f;

            if (!storytellerFocusPS.isEmitting) {
                ParticleSystem.EmissionModule focusLeapEmission = storytellerFocusPS.emission;
                focusLeapEmission.enabled = true;
            }

        } else if (isFocusing && (focus < braveryToUse || Input.GetKeyUp(KeyCode.LeftShift))) {
            isFocusing = false;
            storytellerMovement.GravityMultiplier *= 2f;

            if (storytellerFocusPS.isEmitting) {
                ParticleSystem.EmissionModule focusLeapEmission = storytellerFocusPS.emission;
                focusLeapEmission.enabled = false;
            }
        }

        if (!isFocusing) {
            focus = Mathf.MoveTowards(focus, inspiration, focusRestoreRate * Time.deltaTime);
        } else {
            focus = Mathf.MoveTowards(focus, 0, braveryToUse);
        }

        inspirationSlider.value = inspiration;
        imaginationSlider.value = imagination;
        focusSlider.value = focus;

        if (imagination >= 99.9f && !storytellingTree.IsOpen) {
            storytellingTree.OpenTree();
            imagination -= 100;
        }

	}

    public void GainInspiration(float amount) {
        targetInspiration = inspiration + amount;
        isIncrementingInspiration = true;
    }

    public void StopStorytellerMovement() {
        storytellerMovement.CanMove = false;
    }

    public void StartStorytellerMovement() {
        storytellerMovement.CanMove = true;

        if (storytellerImaginingPS.isPlaying) {
            storytellerImaginingPS.Stop();
        }
    }
}
