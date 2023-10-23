using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject target;
    public GameObject Player;
    public float xOffset, yOffset, zOffset;

    private void Start()
    {
        SetCameraTargetPos(0, 0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + new Vector3(xOffset, yOffset, zOffset);
        transform.LookAt(target.transform.position);
    }

    void SetCameraTargetPos(float xOffset, float  yOffset, float zOffset)
    {
        target.transform.position = Player.transform.position + new Vector3(xOffset,yOffset,zOffset);
    }
}
