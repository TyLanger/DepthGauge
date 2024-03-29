﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer pickaxe;
    public OreGrid grid;
    public GridManager gridManager;
    public int xPos = 5;
    public int yPos = 5;
    public int zPos = 0;

    int desiredX = 0;
    int desiredY = 0;
    int desiredZ = 0;

    float horInput;
    float vertInput;
    int depthInput;
    int xMove;
    int yMove;
    Direction lastDirection;
    int lastAnchorX = 7; // maybe I should do these in start?
    int lastAnchorY = 1;
    int lastAnchorZ = 0;
    public Rope rope;
    bool usingRope = false;
    int lastZLayer;

    public float timeBetweenMovements = 0.1f;
    float timeOfLastMovement = -1;
    public float moveSpeed = 1;
    public float fallSpeed = 3;
    public bool playerControlled = true;

    public int pickPower = 1;
    public int maxPickPower = 3;
    [SerializeField]
    int backPack = 0;
    public int backPackCap = 75;
    //public int maxBackPackCap = 150;
    int[] currentOres;
    [SerializeField]
    int money = 0;
    public float[] oreValues;
    
    public TextFade pickupTextPrefab;
    Queue<TextFade> pickupTextQueue;
    int pickupQueueLength = 3;
    Vector3 pickupOffset = new Vector3(1, 0, -0.5f);

    AudioSource pickHit;
    public float minPitch = 1.5f;
    public float maxPitch = 1.7f;
    //public AudioSource zipSound;

    public event Action<int> OnPlayerChangeLayer;
    public event Action OnRopeStart;
    public event Action OnRopeEnd;
    public event Action<int> OnMoneyChanged;
    public event Action<int, int> OnOreChanged;
    public event Action<int, int> OnPackFillChanged;
    public event Action OnBagFull;

    Vector3 targetPos;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        pickHit = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPos = transform.position;
        currentOres = new int[Enum.GetNames(typeof(OreType)).Length];
        pickupTextQueue = new Queue<TextFade>(pickupQueueLength);
        TextFade copy = Instantiate(pickupTextPrefab);
        pickupTextQueue.Enqueue(copy);

        Ore.OnMined += CollectOre;
    }

    // Update is called once per frame
    void Update()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");
        // even tho I'm clamping input, raw works better
        xMove = (int)Mathf.Clamp(horInput, -1, 1);
        yMove = (int)Mathf.Clamp(vertInput, -1, 1);

        if (xMove != 0)
        {
            transform.localScale = new Vector3(xMove, transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetKey(KeyCode.R))
        {
            depthInput = 1;
        }
        else if(Input.GetKey(KeyCode.F))
        {
            depthInput = -1;
        }
        else
        {
            depthInput = 0;
        }

        if(Input.GetKeyDown(KeyCode.G) && !usingRope)
        {
            UseRope();

        }

        if ((Time.time > timeOfLastMovement + timeBetweenMovements) && (xMove != 0 || yMove != 0 || depthInput != 0) && playerControlled)
        {
            timeOfLastMovement = Time.time;

            desiredX = xPos + xMove;
            //desiredX = Mathf.Clamp(desiredX, 0, grid.xSize - 1);

            desiredY = yPos + yMove;
            //desiredY = Mathf.Clamp(desiredY, 0, grid.ySize - 1);

            desiredZ = zPos + depthInput;
            //desiredZ = Mathf.Clamp(desiredZ, 0, grid.zSize - 1);

            if (gridManager.IsSolid(desiredX, desiredY, desiredZ))
            {
                // try to sttack
                StartCoroutine(Smash(desiredX, desiredY, desiredZ));
            }
            else
            {
                CheckAnchor(xPos, yPos, zPos, desiredX, desiredY, desiredZ);
                xPos = desiredX;
                yPos = desiredY;
                zPos = desiredZ;
                targetPos = gridManager.GetPosition(xPos, yPos, zPos);
                UpdateDepth();
                // move into new square
                UpdateGravity();
            }
        }
        if (!usingRope)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            spriteRenderer.sortingOrder = (int)transform.position.z * -10 + 5; // +5 to stay on top of blocks
            pickaxe.sortingOrder = (int)transform.position.z * -10 + 6;
            int currentZ = (int)transform.position.z;
            if (currentZ != lastZLayer)
            {
                lastZLayer = currentZ;
                OnPlayerChangeLayer?.Invoke(lastZLayer);
            }
        }
        //transform.position = Vector3.MoveTowards(transform.position, )
    }

    private void UseRope()
    {
        if (!usingRope)
        {
            //zipSound.Play();
            usingRope = true;
            OnRopeStart?.Invoke();
            rope.StartPulling();
            lastZLayer = zPos;
        }
    }

    IEnumerator Smash(int x, int y, int z)
    {
        Vector3 smashPos = gridManager.GetPosition(x, y, z);
        Vector3 oldPos = transform.position;
        targetPos = transform.position + (smashPos - transform.position)/2;
        yield return new WaitForSeconds(timeBetweenMovements *0.4f); // slightly faster than time between movements
        Side sideHitFrom = GetSideHitFrom(x, y);
        if (IsBagFull())
        {

        }
        else
        {
            pickHit.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            pickHit.Play();
            gridManager.Smash(x, y, z, pickPower, sideHitFrom);
        }
        targetPos = oldPos;
        UpdateGravity();
    }

    void CollectOre(OreType type, int amount)
    {
        if (amount == 0)
            return;
        currentOres[(int)type] += amount;
        backPack += amount;
        OnPackFillChanged?.Invoke(backPack, backPackCap);
        // +amount type (total)
        //Debug.Log($"Gained {type} ({currentOres[(int)type]})");

        if (pickupTextQueue.Count < pickupQueueLength)
        {
            // create a new one
            TextFade copy = Instantiate(pickupTextPrefab);
            pickupTextQueue.Enqueue(copy);
        }

        TextFade next = pickupTextQueue.Dequeue();
        next.transform.position = transform.position + pickupOffset; // negate when facing left
        next.UpdateColour(type);
        next.SetText($"+{amount} {type} ({currentOres[(int)type]})");
        next.Reset();
        pickupTextQueue.Enqueue(next); // recycle
        //pickupText.text = $"+{amount} {type} ({currentOres[(int)type]})";

        OnOreChanged?.Invoke((int)type, currentOres[(int)type]);

        if (IsBagFull())
        {
            BagFull();
        }
    }

    bool IsBagFull()
    {
        return backPack >= backPackCap;
    }

    void BagFull()
    {
        // maybe wait a sec
        //UseRope();
        OnBagFull?.Invoke();
    }

    public void UpgradePick()
    {
        pickPower++;
    }

    public void UpgradePack(int size)
    {
        backPackCap += size;
        OnPackFillChanged?.Invoke(backPack, backPackCap);
    }

    public bool CanPay(int price)
    {
        return money >= price;
    }

    public void Pay(int price)
    {
        money -= price;
        OnMoneyChanged?.Invoke(money);
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
        pickaxe.sortingOrder = -zPos * 10 + 6;
        OnPlayerChangeLayer?.Invoke(zPos);
    }

    void UpdateGravity()
    {
        if(!gridManager.IsSolid(xPos, yPos-1, zPos))
        {
            //Debug.Log("Gonna fall");
            int newY = yPos;
            while (!gridManager.IsSolid(xPos, newY-1, zPos))
            {
                newY--;
            }
            // fall
            // need to wait for the sprite to be off the edge
            // othewise, you will fall by moving diagonally through the corner

            // find the deepest point
            StartCoroutine(Fall(newY));
        }
    }

    IEnumerator Fall(int newY)
    {
        // is my x and z close to the hole's x and z?
        //if(Mathf.Abs(transform.position.x - startPos.x) < 0.5f)
        CheckAnchor(xPos, yPos, zPos, xPos, newY, zPos);
        Vector3 startPos = transform.position;
        Vector3 holePos = gridManager.GetPosition(xPos, newY, zPos);

        float speed = moveSpeed;
        while(true)
        {
            float x = transform.position.x;
            float z = transform.position.z;
            if(Mathf.Abs(x-holePos.x) < 0.4f && Mathf.Abs(z-holePos.z) < 0.4f)
            {
                // you've moved 0.6 dist from where you started, that means you're over the hole now
                // should probably account for grid spacing...

                yPos = newY;
                targetPos = gridManager.GetPosition(xPos, yPos, zPos);
                playerControlled = false;
                moveSpeed = fallSpeed;
                break;
            }
            yield return null;
        }
        while(true)
        {
            if(Vector3.Distance(transform.position, targetPos) < 0.2f)
            {
                // we landed
                playerControlled = true;
                moveSpeed = speed;
                break;
            }

            yield return null;
        }

        yield return null;
    }

    void CheckAnchor(int oldX, int oldY, int oldZ, int newX, int newY, int newZ)
    {
        int x = newX - oldX;
        if(x != 0)
        {
            // moving in x direction

            // where was the last anchor
            // if it has a different x, that means I'm travelling in a straight line on the x
            // so I don't need an anchor

            // if it is 0, I need an anchor
            if((lastAnchorX - oldX) == 0)
            {
                Debug.Log("Place anchor x");
                PlaceAnchor(oldX, oldY, oldZ);
            }
        }
        int y = newY - oldY;
        if(y != 0)
        {
            // moving in y direction
            if ((lastAnchorY - oldY) == 0)
            {
                Debug.Log("Place anchor y");

                PlaceAnchor(oldX, oldY, oldZ);
            }
        }
        int z = newZ - oldZ;
        if(z != 0)
        {
            // moving in z direction
            if ((lastAnchorZ - oldZ) == 0)
            {
                Debug.Log("Place anchor z");

                PlaceAnchor(oldX, oldY, oldZ);
            }
        }
    }

    void PlaceAnchor(int x, int y, int z)
    {
        lastAnchorX = x;
        lastAnchorY = y;
        lastAnchorZ = z;
        rope.CreateAnchorPoint(gridManager.GetPosition(x, y, z));
    }

    public void ReachedTopOfRope()
    {
        // square the rope is at
        xPos = 7;
        yPos = 1;
        zPos = 0;
        targetPos = GetGridPosition(); // so you stay put
        usingRope = false;

        lastAnchorX = 7;
        lastAnchorY = 1;
        lastAnchorZ = 0;

        UpdateDepth();
        OnRopeEnd?.Invoke();
        // should maybe do a cutscene here
        // also I probably fall in a hole
        // DumpOre()
        money += CalculateMoney();
        OnMoneyChanged?.Invoke(money);
        backPack = 0;
        OnPackFillChanged?.Invoke(backPack, backPackCap);
    }

    int CalculateMoney()
    {
        int sum = 0;
        for (int i = 0; i < currentOres.Length; i++)
        {
            sum += (int)(currentOres[i] * oreValues[i]);
            currentOres[i] = 0;
            OnOreChanged?.Invoke(i, 0);

        }
        return sum;
    }

    // unused
    Direction CalculateDirection(int oldX, int oldY, int oldZ, int newX, int newY, int newZ)
    {
        // only 1 diresction should be different
        int x = newX - oldX;
        if (x > 0)
            return Direction.right;
        if (x < 0)
            return Direction.left;
        // if positive, moving right
        int y = newY - oldY;
        if (y > 0)
            return Direction.up;
        if (y < 0)
            return Direction.down;
        int z = newZ - oldZ;
        if (z > 0)
            return Direction.forward;
        if (z < 0)
            return Direction.back;

        // shouldn't be able to get here.
        // means you didn't move
        return Direction.none;
    }

    public Vector3 GetGridPosition()
    {
        return gridManager.GetPosition(xPos, yPos, zPos);
    }

    enum Direction { none, left, up, right, down, forward, back}; // forward is away from camera
}

