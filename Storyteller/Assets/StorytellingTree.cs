using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorytellingTree : MonoBehaviour {

    [Header("Storytelling Ideas")]
    [SerializeField]
    private List<GameObject> storytellingIdeas;
    [SerializeField]
    private float timeToReachFullShift;

    private float targetShiftScale = 100;
    private float currentShift = 0;
    private float shiftRate;

    private List<StorytellingIdea> currentTierStorytellingIdeas;
    private int currentTier = 0;

    [Header("UI")]
    [SerializeField]
    private Transform leftTransform;
    [SerializeField]
    private Transform rightTransform;
    private bool isOpen = false;
    public bool IsOpen { get { return isOpen; } }

    private ParticleSystem cachedLeftEffect;
    private ParticleSystem cachedRightEffect;

    private void Awake() {
        shiftRate = targetShiftScale / timeToReachFullShift;
        currentTierStorytellingIdeas = new List<StorytellingIdea>();
    }

    private void Update() {
        if (isOpen) {
            bool isShifting = false;
            if (Input.GetKey(KeyCode.LeftArrow)) {
                currentShift -= shiftRate * Time.deltaTime;
                isShifting = true;

                if (!cachedLeftEffect.isPlaying) {
                    cachedLeftEffect.Play();
                }

            } if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                if (cachedLeftEffect.isPlaying) {
                    cachedLeftEffect.Stop();
                }
            }

            if (Input.GetKey(KeyCode.RightArrow)) {
                currentShift += shiftRate * Time.deltaTime;
                isShifting = true;

                if (!cachedRightEffect.isPlaying) {
                    cachedRightEffect.Play();
                }

            } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
                if (cachedRightEffect.isPlaying) {
                    cachedRightEffect.Stop();
                }
            }

            if (!isShifting) {
                currentShift = Mathf.MoveTowards(currentShift, 0, shiftRate * Time.deltaTime);
            }

            if (currentShift < -targetShiftScale) {
                // execute left
                currentTierStorytellingIdeas[0].ExecuteIdea();
                CloseTree();
            }
            if (currentShift > targetShiftScale) {
                // execute right
                currentTierStorytellingIdeas[1].ExecuteIdea();
                CloseTree();
            }
        }
    }

    public void OpenTree() {
        isOpen = true;
        GameController.Instance.StopStorytellerMovement();

        currentShift = 0;
        for (int i = 0; i < storytellingIdeas.Count; i++) {
            StorytellingIdea idea = storytellingIdeas[i].GetComponent<StorytellingIdea>();
            if (idea.IdeaTier == currentTier) {
                currentTierStorytellingIdeas.Add(idea);
            }
        }

        if (currentTierStorytellingIdeas.Count > 0) {
            currentTierStorytellingIdeas[0].transform.position = leftTransform.position;
            currentTierStorytellingIdeas[1].transform.position = rightTransform.position;
            cachedLeftEffect = currentTierStorytellingIdeas[0].GetComponent<ParticleSystem>();
            cachedRightEffect = currentTierStorytellingIdeas[1].GetComponent<ParticleSystem>();
        }

        for (int i = 0; i < currentTierStorytellingIdeas.Count; i++) {
            currentTierStorytellingIdeas[i].StartIdea();
        }
    }

    public void CloseTree() {
        currentShift = 0;
        currentTier++;

        for (int i = 0; i < currentTierStorytellingIdeas.Count; i++) {
            currentTierStorytellingIdeas[i].EndIdea();
        }

        currentTierStorytellingIdeas.Clear();

        isOpen = false;
        GameController.Instance.StartStorytellerMovement();
    }

}
