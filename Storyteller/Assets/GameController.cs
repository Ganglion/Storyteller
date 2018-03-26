using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {

    [Header("Resources")]
    private float inspiration = 150;
    private float imagination = 0;
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
    private Slider inspirationSlider;
    [SerializeField]
    private Slider imaginationSlider;
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

    private void Awake() {
        if (imaginationSuperPS.isPlaying) {
            imaginationSuperPS.Stop();
        }
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

            if (!imaginationSuperPS.isPlaying) {
                imaginationSuperPS.Play();
            }
            if (!storytellerImaginingPS.isPlaying) {
                storytellerImaginingPS.Play();
            }

        } else {
            currentConversionTime = 0;

            if (imaginationSuperPS.isPlaying) {
                imaginationSuperPS.Stop();
            }
        }

        inspirationSlider.value = inspiration;
        imaginationSlider.value = imagination;

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
