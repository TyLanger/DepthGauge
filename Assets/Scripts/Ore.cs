using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{

    int currentHp;
    int maxHp = 5;

    GameObject leftSprite;
    GameObject topSprite;
    GameObject rightSprite;
    GameObject bottomSprite;
    GameObject midSprite;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsSolid()
    {
        //return leftSprite.activeSelf || topSprite.activeSelf || rightSprite.activeSelf || bottomSprite.activeSelf || midSprite.activeSelf;
        return currentHp > 0;
    }

    public void Smash(int power, Side hitFrom)
    {
        // if hit from the left, destroy left sprite first
        gameObject.SetActive(false);
        

        int damageDone = Mathf.Min(currentHp, power);

        switch (hitFrom)
        {
            case Side.Left:
                //SmashFromLeft(damageDone);
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
}

public enum Side { Left, Top, Right, Bottom, Mid};
