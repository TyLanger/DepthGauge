using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform player;
    public Transform crane; // top of the wooden beam
    public Transform craneAnchor; // attaches from the wooden beam to the ground

    public float width;
    public float speed;
    bool pulling = false;
    Vector3 currentTarget;

    LineRenderer line;
    Vector3[] points;

    //int index = 2;

    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[3];
        points[0] = craneAnchor.position;
        points[1] = crane.position;
        points[2] = player.position;

        line = GetComponent<LineRenderer>();
        line.startWidth = width;
        line.endWidth = width;
        line.positionCount = points.Length;
        line.SetPositions(points);
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            line.SetPosition(points.Length-1, player.position);
        }
        if(pulling)
        {
            player.position = Vector3.MoveTowards(player.position, currentTarget, speed * Time.deltaTime);
            if(Vector3.Distance(player.position, currentTarget) < 0.1f)
            {
                ReachedPoint();
            }
        }
    }

    public void CreateAnchorPoint(Vector3 position)
    {
        Vector3[] newPoints = new Vector3[points.Length + 1];

        for (int i = 0; i < newPoints.Length-1; i++)
        {
            newPoints[i] = points[i];
        }
        newPoints[newPoints.Length - 2] = position;
        newPoints[newPoints.Length - 1] = player.position;

        points = newPoints;
        line.positionCount = points.Length;
        line.SetPositions(points);
    }

    public void StartPulling()
    {
        currentTarget = points[points.Length - 2];
        pulling = true;
    }

    void ReachedPoint()
    {
        // move to the next point
        // end when 3 points left: anchor, beam, player
        if (points.Length > 3)
        {
            currentTarget = points[points.Length - 3];
            // prune the mid point?
            Vector3[] newPoints = new Vector3[points.Length - 1];
            for (int i = 0; i < newPoints.Length - 1; i++)
            {
                newPoints[i] = points[i];
            }
            newPoints[newPoints.Length - 1] = player.position; // place the player at the end

            points = newPoints;
            line.positionCount = points.Length;
            line.SetPositions(points);
        }
        else
        {
            pulling = false;
            player.GetComponent<PlayerController>().ReachedTopOfRope();
        }
    }
}
