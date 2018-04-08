using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorytellingArea : MonoBehaviour {

    [SerializeField]
    private string storytellingBlockName;

	private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            FlowchartController.Instance.ExecuteStorytellingBlock(storytellingBlockName);
            gameObject.SetActive(false);
        }
    }

}
