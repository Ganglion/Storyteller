using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {

    [SerializeField]
    private List<GameObject> instructions;
    private int instructionIndex = 0;

	public void ExitToMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Storyteller");
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void Resume() {
        GameController.Instance.ResumeGame();
    }

    public void NextInstruction() {
        instructions[instructionIndex].SetActive(false);
        instructionIndex = (instructionIndex + 1) % instructions.Count;
        instructions[instructionIndex].SetActive(true);
    }
}
