using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public PlayerController player;
    public TextMeshProUGUI money;

    public TextMeshProUGUI[] ores;
    public TextMeshProUGUI bag;

    public TextMeshProUGUI depthX;
    public TextMeshProUGUI depthY;
    public TextMeshProUGUI depthZ;

    public TextMeshProUGUI digaBrass;

    int playerX = 0;
    int playerY = 0;
    int playerZ = 0;

    float playerXOffset = -1.5f;
    float playerYOffset = -6.5f;

    // Start is called before the first frame update
    void Start()
    {
        player.OnMoneyChanged += MoneyUpdate;
        player.OnOreChanged += OreUpdate;
        player.OnPackFillChanged += BagUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        playerX = (int)(player.transform.position.x + playerXOffset);
        playerY = (int)(player.transform.position.y + playerYOffset);
        playerZ = (int)(player.transform.position.z);

        depthX.text = $"x: {playerX}";
        depthY.text = $"y: {playerY}";
        depthZ.text = $"z: {playerZ}";
    }

    void MoneyUpdate(int amount)
    {
        money.text = $"{amount}";
    }

    void OreUpdate(int type, int amount)
    {
        // I created Dugium as the last ore a while ago, but haven't implemented it more than as an enum
        // so gotta check for it
        // whoops
        if (type < ores.Length)
        {
            ores[type].text = $"{amount} {(OreType)type}";
        }
    }

    void BagUpdate(int fill, int max)
    {
        bag.text = $"{fill}/{max} Total";
    }
}
