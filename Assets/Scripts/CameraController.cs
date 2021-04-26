using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    Vector3 targetPoint;
    PlayerController player;
    public Vector3 offset;
    public Vector3 baseOffset;
    public Vector3 minOffset;

    float targetZ = 0;

    public float maxCloseup = -2.5f;
    public float closeScaling = 4f;
    public float moveSpeed = 1;

    bool followingRope = false;

    // Start is called before the first frame update
    void Start()
    {
        baseOffset = offset;
        player = target.GetComponent<PlayerController>();
        player.OnPlayerChangeLayer += PlayerChangeLayer;
        player.OnRopeStart += RopeStart;
        player.OnRopeEnd += RopeEnd;
    }

    // Update is called once per frame
    void Update()
    {
        if(followingRope)
        {
            // as lose as you were when the button was pressed
            transform.position = player.transform.position + offset;
        }
        else
        {
            offset = Vector3.Slerp(baseOffset, minOffset, targetZ / closeScaling);
            targetPoint = player.GetGridPosition();
            //offset = new Vector3(target.position.x, target.position.y, )
            //offset = new Vector3(baseOffset.x, baseOffset.y, Mathf.Lerp(baseOffset.z, maxCloseup, Mathf.Pow((target.position.z / 4f), 2)));
            transform.position = Vector3.MoveTowards(transform.position, targetPoint + offset, Time.deltaTime * moveSpeed);
        }
    }

    void PlayerChangeLayer(int playerZ)
    {
        targetZ = playerZ;
    }

    void RopeStart()
    {
        followingRope = true;
    }

    void RopeEnd()
    {
        followingRope = false;
    }
}
