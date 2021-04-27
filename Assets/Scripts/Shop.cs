using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Shop : Ore
{

    public PlayerController player;
    TextMeshPro tmp;
    //public UnityEvent onSmash; // add which upgrade I want in the editor
    public int price;

    int upgrades = 0;

    public int shopType = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        tmp = GetComponentInChildren<TextMeshPro>();
        tmp.text = $"{price}";
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
                upgrades++;
                price += 125;
                tmp.text = $"{price}";
            }
        }
    }

    public void UpgradePick()
    {
        player.UpgradePick();
    }

    public void UpgradePack()
    {
        // 75 base + 75 = 150
        // + 125 = 275
        // + 175 = 550
        player.UpgradePack(75 + upgrades * 50);
    }
}
