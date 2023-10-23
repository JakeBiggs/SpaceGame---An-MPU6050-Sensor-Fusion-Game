using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class supTest : MonoBehaviour
{

    public SerialCommunicationManager manager;
    public float ledValue;
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        string[] serialRead;
        //serialRead =  manager.ReadPinData(' ');
        /*
        if (serialRead[0] == "14")
        {
            int result = int.Parse(serialRead[1]);
            manager.SerialWriteLine(ledValue.ToString());
        }
        */
    }
}
