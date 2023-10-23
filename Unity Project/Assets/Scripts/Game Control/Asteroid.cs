using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Rigidbody rb;
    public MainHandler mainHandler;
    public float rotationSpeed = 50f;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
       
        transform.Rotate(Vector3.up * rotationSpeed * Time.fixedDeltaTime);
        transform.position += Vector3.back * speed * Time.fixedDeltaTime;
        if (transform.position.z <= -100)
        {
            Destroy(gameObject);
            mainHandler = FindObjectOfType<MainHandler>();
            if (mainHandler.hm.currentHealth >= 1) mainHandler.AddScore(50);
        }   
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            HealthManager healthManager = FindObjectOfType<HealthManager>();
            healthManager.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
