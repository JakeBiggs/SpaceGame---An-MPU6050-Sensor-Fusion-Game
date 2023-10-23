using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageListener : MonoBehaviour
{
    public float ax;
    public float ay;
    public float az;
    public float tmp;
    public float gx;
    public float gy;

    public float gz;
    public PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag;
    }
    void OnMessageArrived(string msg)
    {
        //Debug.Log("Message Recieved: " + msg);
        string[] substrings = msg.Split(",");
        //Debug.Log("Substrings.Length: " + substrings.Length);
        if (!string.IsNullOrEmpty(substrings[0]))
        {
            for (int i = 0; i < substrings.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        ax = float.Parse(substrings[i]);
                        break;
                    case 1:
                        ay = float.Parse(substrings[i]);
                        break;
                    case 2:
                        az = float.Parse(substrings[i]);
                        break;
                    case 3:
                        tmp = float.Parse(substrings[i]);
                        break;
                    case 4:
                        gx = float.Parse(substrings[i]);
                        break;
                    case 5:
                        gy = float.Parse(substrings[i]);
                        break;
                    case 6:
                        gz = float.Parse(substrings[i]);
                        break;
                    default:
                        break;

                }
            }
        }
    }

    void OnConnectionEvent(bool success)
    {
        if (success)
        {
            
            Debug.Log("Connection Established");
        }
        else Debug.Log("Connection Failed");
        
    }

    private void Awake()
    {
       
    }
    private void FixedUpdate()
    {

        playerController.ProcessInputs();
    }

}