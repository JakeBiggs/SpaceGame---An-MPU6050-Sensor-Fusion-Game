using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MainHandler : MonoBehaviour
{
    public HealthManager hm;
    public PlayerController pc;
    public AsteroidSpawner AS;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalscoreText;
    //public CanvasGroup deathCanvas; 
    public int playerScore;
    private float time;
    public string currentScene;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        //hm = GetComponent<HealthManager>();
        //pc = FindObjectOfType<PlayerController>();
        //AS = FindObjectOfType<AsteroidSpawner>();
        //deathCanvas = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
            Debug.Log("Escape pressed");
        }
        if (currentScene != "Main" && currentScene != "Menu" && finalscoreText != null && GlobalControl.Instance != null) 
        {
            finalscoreText.text = "Final Score: "+GlobalControl.Instance.score.ToString();
        }

        time += Time.smoothDeltaTime;

    }

    private void FixedUpdate()
    {
    }

    public void AddScore(int score)
    {
        if (currentScene != "Death") 
        {
            playerScore += score;
            scoreText.text = "Score: " + playerScore.ToString();
        }
    }   
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
        currentScene = scene;
       
        switch (scene)
        {
            case "Menu":
                break;
            case "Main":
                time = 0;
                if (GlobalControl.Instance != null)
                {
                    GlobalControl.Instance.score = 0;
                    GlobalControl.Instance.time = 0;
                }
                hm.currentHealth = hm.maxHealth;
                break;
            case "Death":
               
                int playerScore = PlayerPrefs.GetInt("score");
                
                //finalscoreText.text = "Final Score: " + GlobalControl.Instance.score.ToString();
                
                break;
            
            default:
                break;
        }
    }

    public void SavePlayer()
    {
        GlobalControl.Instance.score = playerScore;
        GlobalControl.Instance.time = Time.time;
    }

    public void Exit()
    {
        Application.Quit();
    }
    
}

