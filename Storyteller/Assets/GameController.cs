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

    [Header("Progress Tree")]
    private GameObject progressTreeCanvas;
	
	void Update () {
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
	}
}
