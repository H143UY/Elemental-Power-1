using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject PausePanel;
    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1.0f;  
    }
    public void Quit()
    {
        Application.Quit();
    }
}
