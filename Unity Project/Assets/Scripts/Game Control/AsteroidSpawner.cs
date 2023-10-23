using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public float spawnRate = 2f; // spawn rate in seconds
    public float asteroidSpeed = 5f; // speed of asteroids falling down the screen
    public Collider playerCollider;
    public MainHandler mainHandler;

    public bool isColliding = false;
    private List<GameObject> asteroids = new List<GameObject>(); // create a list to hold asteroids
    private float timeSinceLastSpawn;
    
    private void Update()
    {
        if (mainHandler.currentScene == "Main") spawnObjects();
    }

    private void spawnObjects()
    {
        // increase timer
        timeSinceLastSpawn += Time.deltaTime;
        if (mainHandler.playerScore < 500) spawnRate = 0.75f;
        else if (mainHandler.playerScore > 500) spawnRate = 0.5f;
        else if (mainHandler.playerScore > 1500) spawnRate = 0.25f;
        else if (mainHandler.playerScore > 3000) spawnRate = 0.1f;
        // spawn new asteroid if timer reaches spawn rate
        if (timeSinceLastSpawn >= spawnRate)
        {
            // reset timer
            timeSinceLastSpawn = 0f;

            // spawn asteroid at a random position
            Vector3 spawnPosition = new Vector3(Random.Range(-160f, 160f), 15f, 110f);
            GameObject newAsteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
            //newAsteroid.tag = "Asteroid"; // set the tag of the new asteroid
            asteroids.Add(newAsteroid); // add the new asteroid to the list

        }
    }

    private void checkCollisions()
    {
        // move all asteroids down the screen
        if (asteroids != null)
        {
            foreach (GameObject asteroid in asteroids)
            {
                //asteroid.transform.Translate(Vector3.back * asteroidSpeed * Time.fixedDeltaTime);
                Rigidbody asteroidRb = asteroid.GetComponent<Rigidbody>();
                asteroidRb.velocity = Vector3.back * asteroidSpeed;

                asteroid.transform.position -= new Vector3(0, 0, asteroidSpeed * Time.deltaTime);

                if (asteroid.transform.position.z < -100f)
                {
                    // remove asteroid from the scene and from the list
                   // mainHandler.AddScore(50);
                    asteroids.Remove(asteroid);
                    
                }

                // check if asteroid collides with the player
                Collider asteroidCollider = asteroid.GetComponent<Collider>(); //Gets collider component
                if (asteroidCollider.bounds.Intersects(playerCollider.bounds)) // do something when the asteroid collides with the player
                {
                    
                    isColliding = true;
                    Debug.Log("Asteroid collided with the player! ");
                    Destroy(asteroid);
                    isColliding = false;
                }
                else
                {
                    isColliding = false;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        
        //checkCollisions();
    }
}
