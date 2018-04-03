using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorytellingTree : MonoBehaviour {

    [Header("Storytelling Ideas")]
    [SerializeField]
    private List<StorytellingIdea> storytellingIdeas;
    [SerializeField]
    private float timeToReachFullShift;

    private float targetShiftScale = 100;
    private float currentShift = 0;
    private float shiftRate;

    private StorytellingIdea currentTierStorytellingIdea;
    private int currentTier = 0;

    [Header("UI")]
    [SerializeField]
    private ParticleSystem leftPS;
    [SerializeField]
    private ParticleSystem rightPS;
    [SerializeField]
    private Transform fillBarScaler;
    [SerializeField]
    private Transform ideaTransform;
    private bool isOpen = false;
    public bool IsOpen { get { return isOpen; } }

    private void Awake() {
        shiftRate = targetShiftScale / timeToReachFullShift;
    }

    private void Update() {
        if (isOpen) {
            if (Input.GetKey(KeyCode.LeftArrow)) {
                currentShift -= shiftRate * Time.deltaTime;

                if (!leftPS.isPlaying) {
                    leftPS.Play();
                } else {
                    //ParticleSystem.MainModule leftPSMain = leftPS.main;
                    //leftPSMain.startSpeed = Mathf.Abs(2 + ((currentShift / targetShiftScale) * 3));
                    ParticleSystem.EmissionModule leftPSEmission = leftPS.emission;
                    leftPSEmission.rateOverTime = Mathf.Abs(currentShift);
                }

            } if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                if (leftPS.isPlaying) {
                    leftPS.Stop();
                }
                currentShift = 0;
            }

            if (Input.GetKey(KeyCode.RightArrow)) {
                currentShift += shiftRate * Time.deltaTime;

                if (!rightPS.isPlaying) {
                    rightPS.Play();
                } else {
                    //ParticleSystem.MainModule rightPSMain = rightPS.main;
                    //rightPSMain.startSpeed = Mathf.Abs(-2 - ((currentShift / targetShiftScale) * 3));
                    ParticleSystem.EmissionModule rightPSEmission = rightPS.emission;
                    rightPSEmission.rateOverTime = Mathf.Abs(currentShift);
                }

            } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
                if (rightPS.isPlaying) {
                    rightPS.Stop();
                }
                currentShift = 0;
            }

            /*
            if (!isShifting) {
                 currentShift = Mathf.MoveTowards(currentShift, 0, shiftRate * Time.deltaTime);
            }*/

            fillBarScaler.localScale = new Vector3(currentShift / targetShiftScale, 1, 1);

            if (currentShift < -targetShiftScale) {
                // execute left
                currentTierStorytellingIdea.ExecuteIdea(0);

                CloseTree();
            }
            if (currentShift > targetShiftScale) {
                // execute right
                currentTierStorytellingIdea.ExecuteIdea(1);

                CloseTree();
            }
        }
    }

    public void OpenTree() {

        GameController.Instance.StopStorytellerMovement();
        isOpen = true;

        currentShift = 0;

        currentTierStorytellingIdea = storytellingIdeas[currentTier];
        currentTierStorytellingIdea.transform.position = ideaTransform.position;

        currentTierStorytellingIdea.StartIdea();

        /*for (int i = 0; i < storytellingIdeas.Count; i++) {
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
        }*/
    }

    public void CloseTree() {
        currentShift = 0;
        currentTier++;

        currentTierStorytellingIdea.EndIdea();

        if (leftPS.isPlaying) {
            leftPS.Stop();
        }
        if (rightPS.isPlaying) {
            rightPS.Stop();
        }

        /*
        for (int i = 0; i < currentTierStorytellingIdeas.Count; i++) {
            currentTierStorytellingIdeas[i].EndIdea();
        }

        currentTierStorytellingIdeas.Clear();*/

        fillBarScaler.localScale = new Vector3(0, 1, 1);

        isOpen = false;
        GameController.Instance.StartStorytellerMovement();
    }

}
