using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Poker;
using System;

public class MyCard : MonoBehaviour
{
    public Sprite[] sprites;
    public Image image;
    public Text text;
    public PlayingCard playingCard { get; private set; }

    public void UpdateContent(PlayingCard playingCard)
    {
        this.playingCard = playingCard;
        UpdateContent();
    }

    public void Reset()
    {
        image.sprite = null;
        text.text = "";
    }

    public void UpdateContent()
    {
        image.sprite = sprites[(int)playingCard.suit];
        text.text = PlayingCard.faceDisplay[playingCard.face];
        StartCoroutine(EnumeratorAnimationScaleUpThenDown());
    }

    public IEnumerator EnumeratorAnimationScaleUpThenDown()
    {
        float target = 1.1f;
        float timer = 0.2f;

        float timeCovered = 0;
        while(timeCovered < timer)
        {
            yield return new WaitForEndOfFrame();
            float frac = timeCovered / timer;
            float currScale = 1 + (target - 1) * frac;
            transform.localScale = Vector3.one * currScale;
            timeCovered += Time.deltaTime;
        }
        while (timeCovered > 0)
        {
            yield return new WaitForEndOfFrame();
            float frac = timeCovered / timer;
            float currScale = 1 + (target - 1) * frac;
            transform.localScale = Vector3.one * currScale;
            timeCovered -= Time.deltaTime;
        }
        transform.localScale = Vector3.one;

    }
}
