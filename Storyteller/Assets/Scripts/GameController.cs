using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {

    [Header("Resources")]
    private float inspiration = 50;
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
    private float focusUseIncreaseRate;
    [SerializeField]
    private float focusRestoreRate;
    [SerializeField]
    private float focusMinUseValue;
    [SerializeField]
    private float focusCooldown;
    private float currentFocusCooldown;
    private float currentFocusUseRate;
    private bool isFocusing = false;
    private bool hasFocused = false;

    [SerializeField]
    private Slider inspirationSlider;
    [SerializeField]
    private Slider imaginationSlider;
    [SerializeField]
    private Slider focusSlider;
    [SerializeField]
    private ParticleSystem imaginationSuperPS;
    [SerializeField]
    private Image focusFillImage;

    [Header("Fireflies")]
    [SerializeField]
    private ParticleSystem firefliesPS;
    [SerializeField]
    private float inspirationThreshold = 37.5f;
    [SerializeField]
    private float baseEmissionRate = .25f;
    [SerializeField]
    private float maxEmissionRate = .375f;

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

    [Header("Path Helper")]
    [SerializeField]
    private ParticleSystem helperPS;
    [SerializeField]
    private float stopRadius = 3;
    [SerializeField]
    private float radiusScale = 0.75f;
    [SerializeField]
    private Camera targetCamera;
    private float targetHelperRadius;
    private Vector3 targetPoint;
    private bool isHelping = false;

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

        targetHelperRadius = Mathf.Min(Screen.width, Screen.height) * radiusScale;

        ParticleSystem.EmissionModule helpingPSEmission = helperPS.emission;
        helpingPSEmission.enabled = false;
    }

    private void Update () {

        if (Input.GetKeyDown(KeyCode.I)) {
            inspiration += 20;
        }

        if (inspiration < inspirationThreshold) {
            ParticleSystem.EmissionModule firefliesEmission = firefliesPS.emission;
            firefliesEmission.rateOverTime = baseEmissionRate + (1 - inspiration / inspirationThreshold) * (maxEmissionRate - baseEmissionRate); 
        } else {
            ParticleSystem.EmissionModule firefliesEmission = firefliesPS.emission;
            firefliesEmission.rateOverTime = 0;
        }

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

        } else {
            currentConversionTime = 0;
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
            imaginationSuperEmission.enabled = false;

        }
        // && !isFocusing && storytellerMovement.Velocity.sqrMagnitude < 25

        if (hasFocused) {
            if (storytellerMovement.Velocity.y <= 0) {
                hasFocused = false;
            }
        }

        if (currentFocusCooldown > 0) {
            currentFocusCooldown -= Time.deltaTime;
        } else {
            focusFillImage.color = Color.white;
        }

        float braveryToUse = currentFocusUseRate * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && focus >= braveryToUse && !hasFocused && currentFocusCooldown <= 0) {
            isFocusing = true;
            currentFocusUseRate = focusMinUseValue;
            storytellerMovement.StartGliding();
            ParticleSystem.EmissionModule focusLeapEmission = storytellerFocusPS.emission;
            focusLeapEmission.enabled = true;

        } else if (isFocusing && (focus < braveryToUse || Input.GetKeyUp(KeyCode.LeftShift))) {
            isFocusing = false;
            storytellerMovement.StopGliding();
            //if (storytellerFocusPS.isEmitting) {
            ParticleSystem.EmissionModule focusLeapEmission = storytellerFocusPS.emission;
            focusLeapEmission.enabled = false;
            currentFocusCooldown = focusCooldown;
            focusFillImage.color = Color.red;
            hasFocused = true;
        }

        if (!isFocusing) {
            if (currentFocusCooldown <= 0) {
                focus = Mathf.MoveTowards(focus, inspiration, focusRestoreRate * Time.deltaTime);
            }
        } else {
            focus = Mathf.MoveTowards(focus, 0, braveryToUse);
            currentFocusUseRate += focusUseIncreaseRate * Time.deltaTime;
        }

        inspirationSlider.value = inspiration;
        imaginationSlider.value = imagination;
        focusSlider.value = focus;

        if (imagination >= 99.9f && !storytellingTree.IsOpen) {
            storytellingTree.OpenTree();
            imagination -= 100;
        }
        
        if (isHelping) {
            Vector3 storytellerScreenPoint = targetCamera.WorldToScreenPoint(storytellerMovement.transform.position);
            Vector3 targetPointScreenPoint = targetCamera.WorldToScreenPoint(targetPoint);
            Vector3 directionToTargetPoint = targetPointScreenPoint - storytellerScreenPoint;
            Vector3 helperPSTargetScreenPoint = storytellerScreenPoint + directionToTargetPoint.normalized * targetHelperRadius;
            helperPSTargetScreenPoint.z = 10;
            helperPS.transform.position = targetCamera.ScreenToWorldPoint(helperPSTargetScreenPoint);
            if (directionToTargetPoint.sqrMagnitude < Mathf.Pow(stopRadius, 2)) {
                StopHelper();
            }
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

        ParticleSystem.EmissionModule imaginingEmission = storytellerImaginingPS.emission;
        imaginingEmission.enabled = false;
        ParticleSystem[] imaginingEmissionChildren = storytellerImaginingPS.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < imaginingEmissionChildren.Length; i++) {
            ParticleSystem.EmissionModule imaginingChildEmission = imaginingEmissionChildren[i].emission;
            imaginingChildEmission.enabled = false;
        }

        ParticleSystem.EmissionModule imaginationSuperEmission = imaginationSuperPS.emission;
        imaginationSuperEmission.enabled = false;
    }

    public void StartHelper(Vector3 point) {
        targetPoint = point;
        ParticleSystem.EmissionModule helpingPSEmission = helperPS.emission;
        helpingPSEmission.enabled = true;
        isHelping = true;
    }

    private void StopHelper() {
        ParticleSystem.EmissionModule helpingPSEmission = helperPS.emission;
        helpingPSEmission.enabled = false;
        isHelping = false;
    }
}
