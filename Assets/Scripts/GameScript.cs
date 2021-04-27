using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public PlayerController player;
    public GameObject Dug;
    public GameObject pick;
    public GameObject depthGauge;
    public GameObject z;
    public GameObject SpeechBubble;
    public TextMeshProUGUI TMP;
    public TextMeshProUGUI digaBrassTMP;
    public float textClearTime = 0.3f;

    public GameObject dugiumObj;
    public RectTransform backdropRect;
    int backdropSize = 530;

    bool identity = false;
    bool elaborate = false;
    bool metDug = false;
    bool hired = false;
    bool PlayerReachedRope = false;
    public Rope rope;

    bool PlayerReachedUnbreakable = false;
    bool playerDugIn = false;

    bool playerBagFull = false;
    bool reachedDeep = false;
    bool minedDigaBrass = false;
    int digaBrass = 0;
    bool gameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        player.OnBagFull += BagFull;
        player.OnRopeEnd += TopOfRope;
        Ore.OnMidMined += MidMined;
    }

    // Update is called once per frame
    void Update()
    {
        if (!identity && player.transform.position.x > -3)
        {
            identity = true;
            OnPlayerIdentityRevealed();
        }
        if (!elaborate && player.transform.position.x > -2)
        {
            elaborate = true;
            OnElaboration();
        }
        if (!metDug && player.transform.position.x > -1)
        {
            metDug = true;
            OnPlayerMetDug();
        }
        if (!hired && player.transform.position.x > 0)
        {
            hired = true;
            OnPlayerHired();
        }
        if (!PlayerReachedRope && player.transform.position.x > 1)
        {
            PlayerReachedRope = true;
            OnPlayerReachedRope();
        }
        if(!PlayerReachedUnbreakable && player.transform.position.y < -5.1f)
        {
            PlayerReachedUnbreakable = true;
            OnPlayerReachedUnbreakable();
        }
        if(!playerDugIn && player.transform.position.z > 1)
        {
            playerDugIn = true;
            z.SetActive(true);
            OnPlayerDugIn();
        }
        if(!reachedDeep && player.transform.position.y < -17f)
        {
            reachedDeep = true;
            OnPlayerReachedDeep();
        }
    }

    void ClearDialogue()
    {
        SpeechBubble.SetActive(false);
    }

    void SendMessage(string message)
    {
        SpeechBubble.SetActive(true);
        TMP.text = message;
    }

    void StopPlayer()
    {
        player.playerControlled = false;
    }

    void StartPlayer()
    {
        player.playerControlled = true;
    }

    void OnPlayerIdentityRevealed()
    {
        StartCoroutine(KidEvent());
    }

    IEnumerator KidEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "Are you a kid? I've been looking for one of those. They're perfectly suited to being miners.";
        SendMessage(message);
    }

    void OnElaboration()
    {
        StartCoroutine(KidEvent2());
    }
    IEnumerator KidEvent2()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "Firstly, they can fit in tight spaces, they're too young to be claustrophobic, and most bestly, they're already minors so no extra training.";
        SendMessage(message);
        //They're perfectly suited to mine work
    }

    void OnPlayerMetDug()
    {
        StartCoroutine(IntroEvent());
    }

    IEnumerator IntroEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "Howdy small stranger, I'm Dug Digadome, owner of this here diggin' hole.";
        SendMessage(message);
    }

    void OnPlayerHired()
    {
        StartCoroutine(HireEvent());
    }

    IEnumerator HireEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "How's about I give you this here pickaxe and you go get me some stuff from this hole? Sound like a deal to you?";
        SendMessage(message);
    }

    void OnPlayerReachedRope()
    {
        //Debug.Log("Reached rope");
        rope.AttachPlayer(player.transform);
        Dug.transform.localScale = new Vector3(1, 1, 1);
        pick.SetActive(true);
        depthGauge.SetActive(true);

        StartCoroutine(RopeEvent());
    }

    IEnumerator RopeEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "Let me just hook you up to this zip rope so I can haul your loot out of the hole.";
        SendMessage(message);
    }

    void OnPlayerReachedUnbreakable()
    {
        StartCoroutine(DigInEvent());
    }

    IEnumerator DigInEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "I see you've run into some Granicrete. That stuff's tough. You're gonna have to go around it. Press R to dig in. C'mon, just squeeze your small body betweeen those rocks.";
        SendMessage(message);
    }

    void OnPlayerDugIn()
    {
        StartCoroutine(DigOutEvent());
    }

    IEnumerator DigOutEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "You can dig back out by pressing F, but only do that if there's something valuable to dig up. All the good stuff is deep and deeper.";
        SendMessage(message);
    }

    void BagFull()
    {
        StartCoroutine(BagFullEvent());
    }

    IEnumerator BagFullEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "Seems like your little pack is full there, eh? Just grab onto the rope with G and it'll zip you out of there.";
        SendMessage(message);
    }

    void TopOfRope()
    {
        if (gameEnded)
        {
            StartCoroutine(TopRopeEndEvent());
        }
        else
        {
            StartCoroutine(TopRopeEvent());
        }
    }

    IEnumerator TopRopeEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "Let's take a gander at what good stuff you brought up. Here's some bubblegum for your troubles. Maybe you can trade it for some upgrades at the camp store over there.";
        SendMessage(message);
    }

    void OnPlayerReachedDeep()
    {
        StartCoroutine(DeepEvent());
        dugiumObj.SetActive(true);
        digaBrassTMP.gameObject.SetActive(true);
        // 382
        backdropRect.sizeDelta = new Vector2(382, backdropSize);      
    }

    IEnumerator DeepEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "Now you're at the good stuff. Try to find some Dugium down there. At the center of it is the good stuff: Diga Brass.";
        SendMessage(message);
    }

    void MidMined(OreType oType)
    {
        if(oType == OreType.Dugium)
        {
            digaBrass++;
            digaBrassTMP.text = $"{digaBrass} Diga Brass";
            if (!gameEnded && digaBrass > 9)
            {
                EndGame();
            }
            if (!minedDigaBrass)
            {
                OnMinedDigaBrass();
            }
        }
    }

    void OnMinedDigaBrass()
    {
        minedDigaBrass = true;
        StartCoroutine(DigaBrassMineEvent());
    }

    IEnumerator DigaBrassMineEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "There she is. Diga Brass. The best material out there for making bullets. I sell this stuff to my cousin in the nearby replica wild west town. I need 10 for the next shipment.";
        SendMessage(message);
    }

    void EndGame()
    {
        gameEnded = true;
        StartCoroutine(End());
    }

    IEnumerator End()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "That's 10 Diga Brass. That should be enough. Haul yourself up here, kid.";
        SendMessage(message);
    }

    IEnumerator TopRopeEndEvent()
    {
        ClearDialogue();
        yield return new WaitForSeconds(textClearTime);
        string message = "The End. Can you imagine if there had been a boss fight with a giant wurm in the deep? And then the Diga Brass you brought up was infested with wurm eggs and Dug got his cummupance for his child labour practises? Man that would've been cool.";
        SendMessage(message);
    }
}
