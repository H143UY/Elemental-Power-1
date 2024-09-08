using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject GameOver;
    private void Awake()
    {
        this.RegisterListener(EventID.PlayerDie, (sender, param) =>
        {
            GameOver.SetActive(true);
        });
    }
    private void Start()
    {
        GameOver.SetActive(false);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
