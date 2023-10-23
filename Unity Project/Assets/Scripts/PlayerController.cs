using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody rb;
    public Transform playerTransform;
    public MessageListener manager;
    public float speed = 10f;
    public Vector3 accelData; public Vector3 gyroData;
    [Range(0f,1f)] public float alpha = 0.95f; //Closer to 1, the more the gyro data is weighted over accel data for the filtered orientation.
 
    public bool IMUEnabled;
    private float accelMagnitude;
    private Vector3 normalisedAccel;
    private Quaternion accelOrientation;
    private Vector3 accelAngle = Vector3.zero;
    private Quaternion gyroOrientation;
    public Quaternion filteredOrientation;
    private Vector3 filteredAngle = Vector3.zero;
    private Vector3 angularVelocity;
    private float deltaTime = 0;
    private Vector3 deltaAngle;
    private Vector3 currentAngle = Vector3.zero;

    private KalmanFilter kalmanX;
    private KalmanFilter kalmanY;
    private KalmanFilter kalmanZ;

    private float ax, ay, az, tmp, gx, gy, gz;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();

        kalmanX = new KalmanFilter(0.01f, 0.5f, 0.1f, 0);
        kalmanY = new KalmanFilter(0.01f, 0.5f, 0.1f, 0);
        kalmanZ = new KalmanFilter(0.01f, 0.5f, 0.1f, 0);

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void FixedUpdate()
    {
        //Movement
        Move();
    }

    public void ProcessInputs()
    {
        if (IMUEnabled)
        {
            ax = manager.ax; ay = manager.ay; az = manager.az; //Gets raw accelerometer data
            gx = manager.gx / 131; gy = manager.gy / 131; gz = manager.gz / 131; //MPU6050 datasheet says dividing gyro data by 131 converrts to degrees per second.

            float kalmanAx = kalmanX.Filter(ax); //Applies Kalman Filter to accelerometer values
            float kalmanAy = kalmanY.Filter(ay);
            float kalmanAz = kalmanZ.Filter(az);

            accelData = new Vector3(kalmanAx, kalmanAy, kalmanAz); //Stores in Vector3s
            gyroData = new Vector3(gx, gy, gz);

            //Acclerometer Orientation
            accelMagnitude = Mathf.Sqrt(Mathf.Pow(kalmanAx, 2) + Mathf.Pow(kalmanAy, 2) + Mathf.Pow(kalmanAz, 2)); // Calculate magnitude of accelerometer data
            normalisedAccel = new Vector3(kalmanAx / accelMagnitude, kalmanAy / accelMagnitude, kalmanAz / accelMagnitude); // Normalize accelerometer data to get a direction vector
            accelAngle.x = Mathf.Acos(Vector3.Dot(normalisedAccel, Vector3.right)) * Mathf.Rad2Deg; // Calculates angle between direction vector and negative x-axis
            accelAngle.y = Mathf.Acos(Vector3.Dot(normalisedAccel, -Vector3.up)) * Mathf.Rad2Deg;
            accelAngle.z = Mathf.Acos(Vector3.Dot(normalisedAccel, Vector3.forward)) * Mathf.Rad2Deg;
            accelAngle = new Vector3(accelAngle.x, accelAngle.y, accelAngle.z); //Creating final accel angle vector

            //accelOrientation = Quaternion.Euler(0, orientationAngleX, accelAngle.y);  // Create quaternion representing device orientation based on accelerometer data
            accelOrientation = Quaternion.Euler(0, 0, -accelAngle.y);  // Create quaternion representing device orientation based on accelerometer data    

            //Gyroscope Orientation 
            angularVelocity = new Vector3(gx, gy, gz); // Get angular velocity from gyroscope data
            deltaTime = Time.deltaTime; // Get time elapsed since last frame
            deltaAngle = angularVelocity * deltaTime; // Calculate change in orientation based on angular velocity and time elapsed
            currentAngle += deltaAngle;  // Add change in orientation to current orientation

            //gyroOrientation = Quaternion.Euler(0, currentAngle.x, currentAngle.y); // Create quaternion representing device orientation based on gyroscope data
            gyroOrientation = Quaternion.Euler(0, 0, -currentAngle.y); // Create quaternion representing device orientation based on gyroscope data

            //Complementary Filter

            //Vector3 filteredAngle = currentAngle * alpha + accelAngle * (1 - alpha);
            filteredAngle = alpha * (currentAngle + deltaAngle) + (1 - alpha) * accelAngle;
            //playerTransform.rotation = Quaternion.Euler(filteredAngle);
            //filteredOrientation = Quaternion.Lerp(accelOrientation, gyroOrientation, alpha).normalized;
            filteredOrientation = Quaternion.Lerp(accelOrientation, gyroOrientation, alpha).normalized;

        }       

    }

    private void Move()
    {
        Vector3 currentPosition = playerTransform.position;
        Quaternion currentRotation = playerTransform.rotation;
        float screenboundX = 170;
        float clampedX = Mathf.Clamp(currentPosition.x, -screenboundX, screenboundX);
        float clampedY = Mathf.Clamp(currentPosition.y, 14, 16);
       // float clampedZRotation = Mathf.Clamp(currentRotation.z, -90, 90);
       // filteredOrientation.z = clampedZRotation;
        playerTransform.rotation = filteredOrientation;
        playerTransform.position = new Vector3(clampedX, clampedY, currentPosition.z);
        
        speed = Mathf.Abs(playerTransform.rotation.z) * 150f;
        if (speed > 100) speed = 100;
        if (playerTransform.rotation.z > 0)
        {
            //playerTransform.Translate(Vector3.left * speed * Time.deltaTime);

            Vector3 direction = new Vector3(-speed * Time.deltaTime, 0, 0);
            playerTransform.Translate(direction);
        }
        else if (playerTransform.rotation.z < 0)
        {
            //playerTransform.Translate(Vector3.right * speed * Time.deltaTime);
            Vector3 direction = new Vector3(speed * Time.deltaTime, 0, 0);
            playerTransform.Translate(direction);
        }
       
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            playerTransform.position += Vector3.left * 120 * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            playerTransform.position += Vector3.right * 120 * Time.fixedDeltaTime;
        }        
    }
}



public class KalmanFilter
{
    private float q;
    private float r;
    private float x;
    private float p;
    private float k;

    public KalmanFilter(float q, float r, float p, float initialValue)
    {
        this.q = q;
        this.r = r;
        this.p = p;
        this.x = initialValue;
    }

    public float Filter(float measurement)
    {
        p = p + q;

        k = p / (p + r);
        x = x + k * (measurement - x);
        p = (1 - k) * p;

        return x;
    }
}
