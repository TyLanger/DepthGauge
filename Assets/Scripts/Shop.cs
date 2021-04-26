using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shop : Ore
{

    public PlayerController player;
    //public UnityEvent onSmash; // add which upgrade I want in the editor
    public int price;

    public int shopType = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public override void Smash(int power, Side hitFrom, bool obliterate = false)
    {
        // don't do any smashing
        // just open the shop
        // maybe I'll have multiple shops. You smash up to buy stuff
        if (!obliterate)
        {
            Debug.Log("Tried to buy something");
            if (player.CanPay(price))
            {
                player.Pay(price);
                if(shopType == 0)
                {
                    UpgradePick();
                }
                else
                {
                    UpgradePack();
                }
            }
        }
    }

    public void UpgradePick()
    {
        player.UpgradePick();
    }

    public void UpgradePack()
    {
        player.UpgradePack();
    }
}
