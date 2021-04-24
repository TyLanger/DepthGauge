using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public OreGrid grid;
    public int xPos = 5;
    public int yPos = 5;
    public int zPos = 0;

    int desiredX = 0;
    int desiredY = 0;
    int desiredZ = 0;

    float horInput;
    float vertInput;

    public float timeBetweenMovements = 0.1f;
    float timeOfLastMovement = -1;
    public float moveSpeed = 1;

    Vector3 targetPos;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");
        // even tho I'm clamping input, raw works better


        if (Time.time > timeOfLastMovement + timeBetweenMovements)
        {
            int xMove = (int)Mathf.Clamp(horInput, -1, 1);
            int yMove = (int)Mathf.Clamp(vertInput, -1, 1);
            if (xMove != 0 || yMove != 0)
            {
                timeOfLastMovement = Time.time;
            }
            desiredX = xPos + xMove;
            desiredX = Mathf.Clamp(desiredX, 0, grid.xSize - 1);

            desiredY = yPos + yMove;
            desiredY = Mathf.Clamp(desiredY, 0, grid.ySize - 1);

            if (grid.IsSolid(desiredX, desiredY, desiredZ))
            {
                // try to sttack
                StartCoroutine(Smash(desiredX, desiredY, desiredZ));
            }
            else
            {
                xPos = desiredX;
                yPos = desiredY;
                zPos = desiredZ;
                targetPos = grid.GetPosition(xPos, yPos, zPos);
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed*Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, )
    }

    IEnumerator Smash(int x, int y, int z)
    {
        Vector3 smashPos = grid.GetPosition(x, y, z);
        Vector3 oldPos = transform.position;
        targetPos = transform.position + (smashPos - transform.position)/2;
        yield return new WaitForSeconds(timeBetweenMovements *0.4f); // slightly faster than time between movements
        Side sideHitFrom = GetSideHitFrom(x, y);
        Debug.Log($"Hit from {sideHitFrom}");
        grid.Smash(x, y, z, 5, sideHitFrom);
        targetPos = oldPos;
    }

    Side GetSideHitFrom(int blockX, int blockY)
    {
        Side side = Side.Mid;
        if(blockX > xPos)
        {
            side = Side.Left;
        }
        else if(blockX < xPos)
        {
            side = Side.Right;
        }
        else if(blockY > yPos)
        {
            side = Side.Bottom;
        }
        else if(blockY < yPos)
        {
            side = Side.Top;
        }
        return side;
    }

    void UpdateDepth()
    {
        spriteRenderer.sortingOrder = -zPos * 10 + 5; // +5 to stay on top of blocks
    }
}
