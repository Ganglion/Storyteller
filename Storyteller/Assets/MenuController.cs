using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [SerializeField]
    private GameObject credits;
    [SerializeField]
    private GameObject instructionsDisplay;
    [SerializeField]
    private List<GameObject> instructions;
    private int instructionIndex = 0;

    public void StartGame() {
        SceneManager.LoadScene("Chapter One");
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void ToggleCredits() {
        credits.SetActive(!credits.activeInHierarchy);
    }

    public void ShowInstructions() {
        instructionsDisplay.SetActive(true);
    }

    public void NextInstruction() {
        instructions[instructionIndex].SetActive(false);
        instructionIndex = (instructionIndex + 1) % instructions.Count;
        instructions[instructionIndex].SetActive(true);
        if (instructionIndex == 0) {
            instructionsDisplay.SetActive(false);
        }
    }
}
