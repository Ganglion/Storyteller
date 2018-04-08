using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [SerializeField]
    private GameObject credits;

	public void StartGame() {
        SceneManager.LoadScene("Chapter One");
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void ToggleCredits() {
        credits.SetActive(!credits.activeInHierarchy);
    }
}
