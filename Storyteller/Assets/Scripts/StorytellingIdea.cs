using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorytellingIdea : MonoBehaviour {

    /*
    [SerializeField]
    private int ideaTier;
    public int IdeaTier { get { return ideaTier; } }
    */
    [SerializeField]
    private List<GameObject> spawnables;
    public List<GameObject> Spawnables { get { return spawnables; } }

    public virtual void StartIdea() {
        GetComponent<Image>().enabled = true;
    }

    public virtual void ExecuteIdea(int index) {
        if (index < spawnables.Count) {
            spawnables[index].SetActive(true);
            GameController.Instance.StartHelper(spawnables[index].transform.position);
        }
    }

    public virtual void EndIdea() {
        GetComponent<Image>().enabled = false;
    }

}
