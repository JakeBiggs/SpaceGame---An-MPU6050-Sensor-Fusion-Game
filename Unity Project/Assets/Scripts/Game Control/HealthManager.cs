using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HealthManager : MonoBehaviour
{
    public GameObject player;
    public AsteroidSpawner spawner;
    public MainHandler mainHandler;
    public TextMeshProUGUI textMeshPro;
    public int currentHealth;
    public int maxHealth;
 

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 5;
        maxHealth = currentHealth;
        mainHandler = FindObjectOfType<MainHandler>();
     
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth >= 1 && mainHandler.currentScene == "Main")
        {
            if (Input.GetKeyDown(KeyCode.L)) TakeDamage(1);
            textMeshPro.text = "Lives : " + currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        
        if (currentHealth > 1)
        {
            
            currentHealth -= damage;
        }
        else
        {
            mainHandler.SavePlayer();
            mainHandler.LoadScene("Death");
        }
    }
}
