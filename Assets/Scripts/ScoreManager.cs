using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TMP_Text timerText;
    public TMP_Text timer2Text;
    
    public UnityEvent OnGameOver;
    
    [HideInInspector]
    public float score;

    public float scoreAccumulationSpeed;

    public List<NPC> npcs;
    

    void Start()
    {
        ScoreManager.Instance = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        int deadNPCs = 0;
        foreach (NPC npc in npcs)
        {
            if (npc.isDead) deadNPCs++;
        }

        int aliveNPCS = npcs.Count - deadNPCs;

        if (deadNPCs == npcs.Count)
        {
            timerText.text = "Game Over \n"+((int)score);
            timer2Text.text = "Game Over \n"+((int)score);
            
            // TODO: Do game over logic
            OnGameOver.Invoke();
        }
        else
        {
            score += Time.deltaTime * scoreAccumulationSpeed * aliveNPCS;
            timerText.text = ((int)score).ToString();
            timer2Text.text = ((int)score).ToString();
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
