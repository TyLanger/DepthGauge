using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ore : MonoBehaviour
{

    int currentHp;
    int maxHp = 5;
    public int hardness = 0; // high hardness takes stronger pick/more hits

    public GameObject leftSprite;
    public GameObject topSprite;
    public GameObject rightSprite;
    public GameObject bottomSprite;
    public GameObject midSprite;

    public OreType oreType;
    public static event Action<OreType, int> OnMined;

    // Start is called before the first frame update
    void Awake()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSortOrder(int order)
    {
        // 0 or -10 or -20...
        SpriteRenderer left = leftSprite.GetComponent<SpriteRenderer>();
        if(left)
            left.sortingOrder = order;
        SpriteRenderer top = topSprite.GetComponent<SpriteRenderer>();
        if(top)
            top.sortingOrder = order;
        SpriteRenderer right = rightSprite.GetComponent<SpriteRenderer>();
        if(right)
            right.sortingOrder = order;
        SpriteRenderer bottom = bottomSprite.GetComponent<SpriteRenderer>();
        if(bottom)
            bottom.sortingOrder = order;
        SpriteRenderer mid = midSprite.GetComponent<SpriteRenderer>();
        if(mid)
            mid.sortingOrder = order;
    }

    public bool IsSolid()
    {
        //return leftSprite.activeSelf || topSprite.activeSelf || rightSprite.activeSelf || bottomSprite.activeSelf || midSprite.activeSelf;
        return currentHp > 0;
    }

    public void Smash(int power, Side hitFrom)
    {
        //Debug.Log($"smash2? {power} {hitFrom}");
        // if hit from the left, destroy left sprite first
        power = Mathf.Clamp(power-hardness, 0, maxHp);
        int damageDone = Mathf.Min(currentHp, power);
        if (damageDone > 0)
        {
            switch (hitFrom)
            {
                case Side.Left:
                    SmashFromLeft(damageDone);
                    break;

                case Side.Top:
                    SmashFromTop(damageDone);
                    break;

                case Side.Right:
                    SmashFromRight(damageDone);
                    break;

                case Side.Bottom:
                    SmashFromBottom(damageDone);
                    break;

                case Side.Mid:
                    SmashFromSide(damageDone);
                    break;
            }
        }
        else
        {
            // ore was too hard
            // send a message?
            //return 0?
        }
        currentHp -= damageDone;
        OnMined?.Invoke(oreType, damageDone);
    }

    void SmashFromLeft(int damageDone)
    {

        // hit from the left
        for (int i = 0; i < damageDone; i++)
        {
            if (leftSprite.activeSelf)
            {
                leftSprite.SetActive(false);
                continue;
            }

            // how to randomize?
            if (topSprite.activeSelf)
            {
                topSprite.SetActive(false);
                continue;
            }
            if (midSprite.activeSelf)
            {
                midSprite.SetActive(false);
                continue;
            }
            if (bottomSprite.activeSelf)
            {
                bottomSprite.SetActive(false);
                continue;
            }

            if (rightSprite.activeSelf)
            {
                rightSprite.SetActive(false);
            }

        }
    }
    void SmashFromTop(int damageDone)
    {

        for (int i = 0; i < damageDone; i++)
        {
            if (topSprite.activeSelf)
            {
                topSprite.SetActive(false);
                continue;
            }

            // how to randomize?
            if (leftSprite.activeSelf)
            {
                leftSprite.SetActive(false);
                continue;
            }
            if (midSprite.activeSelf)
            {
                midSprite.SetActive(false);
                continue;
            }
            if (rightSprite.activeSelf)
            {
                rightSprite.SetActive(false);
                continue;
            }

            if (bottomSprite.activeSelf)
            {
                bottomSprite.SetActive(false);
            }

        }
    }

    void SmashFromRight(int damageDone)
    {

        for (int i = 0; i < damageDone; i++)
        {
            if (rightSprite.activeSelf)
            {
                rightSprite.SetActive(false);
                continue;
            }

            // how to randomize?
            if (topSprite.activeSelf)
            {
                topSprite.SetActive(false);
                continue;
            }
            if (midSprite.activeSelf)
            {
                midSprite.SetActive(false);
                continue;
            }
            if (bottomSprite.activeSelf)
            {
                bottomSprite.SetActive(false);
                continue;
            }

            if (leftSprite.activeSelf)
            {
                leftSprite.SetActive(false);
            }

        }
    }

    void SmashFromBottom(int damageDone)
    {

        for (int i = 0; i < damageDone; i++)
        {
            if (bottomSprite.activeSelf)
            {
                bottomSprite.SetActive(false);
                continue;
            }

            // how to randomize?
            if (leftSprite.activeSelf)
            {
                leftSprite.SetActive(false);
                continue;
            }
            if (midSprite.activeSelf)
            {
                midSprite.SetActive(false);
                continue;
            }
            if (rightSprite.activeSelf)
            {
                rightSprite.SetActive(false);
                continue;
            }

            if (topSprite.activeSelf)
            {
                topSprite.SetActive(false);
            }

        }
    }

    void SmashFromSide(int damageDone)
    {

        for (int i = 0; i < damageDone; i++)
        {
            if (midSprite.activeSelf)
            {
                midSprite.SetActive(false);
                continue;
            }

            // how to randomize?
            if (leftSprite.activeSelf)
            {
                leftSprite.SetActive(false);
                continue;
            }
            if (topSprite.activeSelf)
            {
                topSprite.SetActive(false);
                continue;
            }
            if (rightSprite.activeSelf)
            {
                rightSprite.SetActive(false);
                continue;
            }

            if (bottomSprite.activeSelf)
            {
                bottomSprite.SetActive(false);
            }

        }
    }
}

public enum Side { Left, Top, Right, Bottom, Mid};
// Granicrete, Cobblite, Ironium/Ferrium, Mithium, Aurite/Gold
public enum OreType { Dirt, Cobblite, Bouldite, Ferrium, Mithium, Dugium};
