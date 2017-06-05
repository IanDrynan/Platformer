using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour {

    public GameManager gm;
    public GameObject pausePanel;
    

	public void LoadThis(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    public void LoadNext()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex, LoadSceneMode.Single);        
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
    public void showPanel(bool end)
    {
       pausePanel.SetActive(true);
        if (end)
        {
            pausePanel.transform.Find("Next Level").gameObject.SetActive(true);
        }
        else
        {
            pausePanel.transform.Find("Next Level").gameObject.SetActive(false);
            gm.pauseGame();
        }
    }

    public void closePanel()
    {
        pausePanel.SetActive(false);
        gm.resumeGame();
    }

    //reset PlayerPrefs for testing only
    public void reset()
    {
        PlayerPrefs.DeleteAll();
    }
}
