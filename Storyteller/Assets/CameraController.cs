using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController> {

	private Transform cameraGroupTransform;
	private Vector3 initialCameraPosition;
	private List<ShakeInstance> shakeInstances;
    //private Camera cameraComponent;

    [Header("Follow")]
    [SerializeField]
    private Transform followObject;
    [SerializeField]
    private float maxFollowSpeed;
    [SerializeField]
    private float followAcceleration;

    /*
    [Header("Field of View")]
    private float initialCameraFOV;
    private float targetCameraFOV;
    [SerializeField]
    private float fovRateOfChange;
    */

	void Awake() {
        cameraGroupTransform = transform.GetChild (0).transform;
		initialCameraPosition = cameraGroupTransform.localPosition;
		shakeInstances = new List<ShakeInstance> ();
        //cameraComponent = cameraTransform.GetComponent<Camera>();

        /*
        initialCameraFOV = cameraComponent.fieldOfView;
        targetCameraFOV = initialCameraFOV;
        */
    }

	void Update() {

        // Shake
		float currentShakeIntensity = 0;
		for (int i = 0; i < shakeInstances.Count; i++) {
			ShakeInstance currentShakeInstance = shakeInstances [i];
			if (currentShakeInstance.Duration <= 0) {
				shakeInstances.RemoveAt (i);
			} else {
				currentShakeIntensity = Mathf.Max (currentShakeIntensity, currentShakeInstance.Intensity * currentShakeInstance.Duration / currentShakeInstance.FullDuration);
				currentShakeInstance.Duration = currentShakeInstance.Duration - Time.unscaledDeltaTime;
			}
		}
        if (shakeInstances.Count > 0) {
            cameraGroupTransform.localPosition = initialCameraPosition + Random.insideUnitSphere * currentShakeIntensity;
        } else {
            cameraGroupTransform.localPosition = initialCameraPosition;
        }

        /*
        // FoV
        cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, targetCameraFOV, fovRateOfChange * Time.deltaTime);
        */

        // Follow

        transform.position = Vector3.Lerp(transform.position, followObject.position, maxFollowSpeed * Time.deltaTime);

    }

	public void ShakeCamera(float intensity, float duration) {
		ShakeInstance newShakeInstance = new ShakeInstance (intensity, duration);
		shakeInstances.Add (newShakeInstance);
	}

    /*
    public void SetCameraFOV(float fov) {
        targetCameraFOV = fov;

    }

    public void ResetCameraScale() {
        targetCameraFOV = initialCameraFOV;
    }
    */

}
