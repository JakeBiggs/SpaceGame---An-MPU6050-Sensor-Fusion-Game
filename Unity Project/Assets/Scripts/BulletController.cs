using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1f;

    private void Start()
    {
        // set the initial velocity of the bullet in the forward direction
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // do something when the bullet collides with another object
        Debug.Log("Bullet hit " + other.gameObject.name);

        // destroy the bullet
        Destroy(gameObject);
    }
}