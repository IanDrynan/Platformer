using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	public void LoadThis(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }
}
