using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel, howToPlayPanel;
    

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
