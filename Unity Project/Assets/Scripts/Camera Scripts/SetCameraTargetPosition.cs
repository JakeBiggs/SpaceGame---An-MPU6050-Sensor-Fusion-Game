using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraTargetPosition : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject self;
    public float offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        //self.transform.position.z  = playerObject.transform.position.z + offset;
    }
}
