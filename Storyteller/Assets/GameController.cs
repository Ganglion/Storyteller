using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {

    [Header("Resources")]
    private float inspiration = 100;
    private float imagination = 0;
    [SerializeField]
    private float baseConversionSpeed;
    [SerializeField]
    private float maxConversionSpeed;
    [SerializeField]
    private float timeToReachMaxConversionSpeed;
    private float currentConversionTime = 0;

    [SerializeField]
    private float baseShakeIntensity;
    [SerializeField]
    private float maxShakeIntensity;

    [SerializeField]
    private Slider inspirationSlider;
    [SerializeField]
    private Slider imaginationSlider;

    [Header("Progress Tree")]
    [SerializeField]
    private StorytellingTree storytellingTree;

    [Header("Storyteller")]
    [SerializeField]
    private ObjectMovement storytellerMovement;
	
	private void Update () {
		if (Input.GetKey(KeyCode.Space) && inspiration > 0) {
            currentConversionTime += Time.deltaTime;
            float percentCharge = Mathf.Clamp01(currentConversionTime / timeToReachMaxConversionSpeed);
            float currentConversionSpeed = baseConversionSpeed + (maxConversionSpeed - baseConversionSpeed) * percentCharge;
            float currentShakeIntensity = baseShakeIntensity + (maxShakeIntensity - baseShakeIntensity) * percentCharge;
            CameraController.Instance.ShakeCamera(currentShakeIntensity, 0.05f);
            float conversionAmount = Mathf.Min(currentConversionSpeed * Time.deltaTime, inspiration);
            inspiration -= conversionAmount;
            imagination += conversionAmount;

        } else if (Input.GetKeyUp(KeyCode.Space)) {
            currentConversionTime = 0;
        }

        inspirationSlider.value = inspiration;
        imaginationSlider.value = imagination;

        if (imagination >= 99.9f && !storytellingTree.IsOpen) {
            storytellingTree.OpenTree();
            imagination -= 100;
        }

	}

    public void StopStorytellerMovement() {
        storytellerMovement.CanMove = false;
    }

    public void StartStorytellerMovement() {
        storytellerMovement.CanMove = true;
    }
}
