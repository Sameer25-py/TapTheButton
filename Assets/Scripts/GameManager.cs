using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject GameplayCanvas, ScrollCanvas, MainMenuCanvas, RulesCanvas, SettingsCanvas, CollectionCanvas;

    public int LastUnlockedScroll = 0;

    public TapButton TapButton;

    private bool _isVibrate = true;

    public AudioSource AudioSource;

    public List<string> ScrollPrompts; 

    private void OnEnable()
    {
        TapButton.UpdateScroll.AddListener(OnUpdateScrollCalled);
        TapButton.Vibrate.AddListener(OnVibrateCalled);
    }

    private void OnVibrateCalled()
    {
        if (_isVibrate)
        {
            Handheld.Vibrate();
        }
    }

    private void OnUpdateScrollCalled()
    {
        LastUnlockedScroll += 1;
        UpdateScrollUI();
    }

    private void Start()
    {
        HomeButton();
    }

    private void UpdateScrollUI()
    {
        ScrollCanvas.GetComponentInChildren<TMP_Text>()
            .text = LastUnlockedScroll + "/21";
    }

    public void HomeButton()
    {
        MainMenuCanvas.SetActive(true);
        GameplayCanvas.SetActive(false);
        RulesCanvas.SetActive(false);
        SettingsCanvas.SetActive(false);
        CollectionCanvas.SetActive(false);
        ScrollCanvas.SetActive(false);
    }

    public void PlayButton()
    {
        UpdateScrollUI();
        if (LastUnlockedScroll >= ScrollPrompts.Count)
        {
            LastUnlockedScroll = ScrollPrompts.Count - 1;
        }
        TapButton.ScrollTxt    = ScrollPrompts[LastUnlockedScroll];
        TapButton.ScrollNumber = LastUnlockedScroll;
        MainMenuCanvas.SetActive(false);
        GameplayCanvas.SetActive(true);
        ScrollCanvas.SetActive(true);
    }

    public void RulesButton()
    {
        MainMenuCanvas.SetActive(false);
        RulesCanvas.SetActive(true);
    }

    public void SettingsButton()
    {
        MainMenuCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
    }

    public void CollectionsButton()
    {
        MainMenuCanvas.SetActive(false);
        CollectionCanvas.SetActive(true);
    }


    public void ChangeSoundState(bool status)
    {
        AudioSource.mute = !status;
    }

    public void ChangeVibrationState(bool status)
    {
        _isVibrate = status;
    }
}