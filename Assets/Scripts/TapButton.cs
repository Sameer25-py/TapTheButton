using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TapButton : MonoBehaviour
{
    public GameObject    Button;
    public RectTransform RectTransform;
    public Image         InitialBaloon, FillBaloon;
    public Vector2       InitialPosition    = new Vector2(0f, -180f);
    public Vector2       FinalPosition      = Vector2.zero;
    public Vector2       InitialSize        = Vector2.zero;
    public Vector2       FinalSize          = Vector2.one;
    public int           TotalButtonPress   = 5;
    public int           CurrentButtonPress = 0;
    public float         DelayDuration      = 0.5f;
    public Vector2       ScrollRevealPosition;
    public TMP_Text      ScrollText;

    public Image      InitialScroll, UnfoldScroll;
    public GameObject OkButton,      HomeButtonBottom, HomeButtonTop;
    public int        ScrollNumber = 1;
    public string     ScrollTxt;
    public TMP_Text   ScrollNumberText;
    public GameObject ReturnUI;

    private float elapsedTime                = 0f;
    private bool  triggerScrollAnimationOnce = false;

    public Color ActiveColor, DisableColor, DimColor;
    
    public static readonly UnityEvent UpdateScroll = new();
    public static readonly UnityEvent Vibrate      = new();

    public AudioSource AudioSource;

    public AudioClip InflateBalloonClip, BalloonBurstClip, UnfoldScrollClip;

    private void OnEnable()
    {
        Initialize();
        
    }
    
    private void Initialize()
    {
        RectTransform.anchoredPosition = InitialPosition;
        RectTransform.localScale       = InitialSize;
        InitialBaloon.color            = ActiveColor;
        FillBaloon.color               = DisableColor;
        InitialScroll.color            = DisableColor;
        UnfoldScroll.color             = DisableColor;
        ScrollText.gameObject.SetActive(false);
        Button.SetActive(true);
        OkButton.SetActive(false);
        HomeButtonBottom.SetActive(false);
        HomeButtonTop.SetActive(true);
        CurrentButtonPress = 0;
        elapsedTime        = 0f;
        ScrollNumberText.gameObject.SetActive(false);
        ReturnUI.SetActive(false);
    }

    public void ButtonPress()
    {   
        Vibrate?.Invoke();
        AudioSource.clip = InflateBalloonClip;
        AudioSource.Play();
        if (CurrentButtonPress == 0)
        {
            FillBaloon.color    = ActiveColor;
            InitialBaloon.color = DisableColor;
        }

        InflateBalloon();
        elapsedTime = 0f;
    }

    public void OKButtonPressed()
    {
        OkButton.SetActive(false);
        HomeButtonBottom.SetActive(true);
        FillBaloon.color   = DisableColor;
        UnfoldScroll.color = DisableColor;
        ScrollText.gameObject.SetActive(false);
        ReturnUI.SetActive(true);
        ScrollNumberText.gameObject.SetActive(false);
        UpdateScroll?.Invoke();
    }
    
    private void InflateBalloon()
    {
        if (++CurrentButtonPress > TotalButtonPress)
        {
            triggerScrollAnimationOnce = true;
            PlayScrollAnimation();
            return;
        }

        RectTransform.anchoredPosition = Vector2.Lerp(InitialPosition, FinalPosition,
            CurrentButtonPress * 1f / TotalButtonPress);
        RectTransform.localScale = Vector2.Lerp(InitialSize, FinalSize,
            CurrentButtonPress * 1f / TotalButtonPress);
    }

    private void DeflateBalloon()
    {
        if (CurrentButtonPress == 0 || CurrentButtonPress == TotalButtonPress) return;
        CurrentButtonPress -= 1;
        RectTransform.anchoredPosition = Vector2.Lerp(InitialPosition, FinalPosition,
            CurrentButtonPress * 1f / TotalButtonPress);
        RectTransform.localScale = Vector2.Lerp(InitialSize, FinalSize,
            CurrentButtonPress * 1f / TotalButtonPress);
        if (CurrentButtonPress == 0)
        {
            FillBaloon.color           = DisableColor;
            InitialBaloon.color        = ActiveColor;
            triggerScrollAnimationOnce = false;
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= DelayDuration)
        {
            DeflateBalloon();
            elapsedTime = 0f;
        }
    }

    private IEnumerator RevealScroll()
    {
        AudioSource.clip = BalloonBurstClip;
        AudioSource.Play();
        yield return new WaitForSeconds(0.5f);
        float         time          = 0f;
        RectTransform rectTransform = InitialScroll.GetComponent<RectTransform>();
        while (time < 1f)
        {
            time                           += Time.deltaTime / 0.5f;
            rectTransform.anchoredPosition =  Vector2.Lerp(Vector2.zero, ScrollRevealPosition, time);
            yield return null;

        }
        InitialScroll.color     = DisableColor;
        UnfoldScroll.fillAmount = 0f;
        UnfoldScroll.color      = ActiveColor;
        time                    = 0f;

        AudioSource.clip = UnfoldScrollClip;
        AudioSource.Play();
        
        while (time < 1f)
        {
            time                    += Time.deltaTime;
            UnfoldScroll.fillAmount =  Mathf.Lerp(0f, 1f, time);
            yield return null;
        }
        
        RevealText();
    }

    private void RevealText()
    {
        ScrollText.gameObject.SetActive(true);
        OkButton.SetActive(true);
        Button.SetActive(false);
        HomeButtonTop.SetActive(false);
        ScrollNumberText.text = "#" + (ScrollNumber + 1);
        ScrollText.text       = ScrollTxt;
        ScrollNumberText.gameObject.SetActive(true);
    }

    private void PlayScrollAnimation()
    {
        if (triggerScrollAnimationOnce)
        {
            triggerScrollAnimationOnce = false;
            InitialScroll.color        = ActiveColor;
            FillBaloon.color           = DimColor;
            Button.SetActive(false);
            StartCoroutine(RevealScroll());
        }
    }
}