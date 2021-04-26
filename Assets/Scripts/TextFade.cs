using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
    public float lifetime;
    float originalLifetime;
    public float riseSpeed = 1;
    public float fadeSpeed = 0.5f;

    TextMeshPro tmp;

    public Color[] oreColours;

    // Start is called before the first frame update
    void Awake()
    {
        originalLifetime = lifetime;
        tmp = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, Time.deltaTime * riseSpeed);
            float percent = 1- lifetime / originalLifetime; 
            float alpha = 1-percent * percent * percent;
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);
        }
    }

    public void SetText(string text)
    {
        tmp.text = text;
    }

    public void UpdateColour(OreType ore)
    {
        tmp.color = oreColours[(int)ore];
    }

    public void Reset()
    {
        lifetime = originalLifetime;
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1);
    }
}
