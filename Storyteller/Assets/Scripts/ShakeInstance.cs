using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeInstance {
	
	private float intensity;
	private float duration;
    private float fullDuration;
	public float Duration { get { return duration; } set { duration = value; } }
	public float Intensity { get { return intensity; } }
    public float FullDuration { get { return fullDuration; } }

    public ShakeInstance(float shakeIntensity, float shakeDuration) {
		intensity = shakeIntensity;
		duration = shakeDuration;
        fullDuration = shakeDuration;
	}

}
