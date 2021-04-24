using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{

    int currentHp;
    int maxHp = 5;

    public GameObject leftSprite;
    public GameObject topSprite;
    public GameObject rightSprite;
    public GameObject bottomSprite;
    public GameObject midSprite;

    // Start is called before the first frame update
    void Start()
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
        left.sortingOrder = order;
        SpriteRenderer top = topSprite.GetComponent<SpriteRenderer>();
        top.sortingOrder = order;
        SpriteRenderer right = rightSprite.GetComponent<SpriteRenderer>();
        right.sortingOrder = order;
        SpriteRenderer bottom = bottomSprite.GetComponent<SpriteRenderer>();
        bottom.sortingOrder = order;
        SpriteRenderer mid = midSprite.GetComponent<SpriteRenderer>();
        mid.sortingOrder = order;
    }

    public bool IsSolid()
    {
        //return leftSprite.activeSelf || topSprite.activeSelf || rightSprite.activeSelf || bottomSprite.activeSelf || midSprite.activeSelf;
        return currentHp > 0;
    }

    public void Smash(int power, Side hitFrom)
    {
        // if hit from the left, destroy left sprite first
        
        int damageDone = Mathf.Min(currentHp, power);

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
        currentHp -= damageDone;
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
