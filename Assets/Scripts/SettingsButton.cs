using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public  Sprite EnableSprite, DisableSprite;
    private bool   _state = true;
    private Button _button;

    public UnityEvent<bool> ButtonClicked;

    private void Start()
    {
        GetComponent<Image>()
            .sprite = EnableSprite;
    }

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        _state = !_state;
        GetComponent<Image>()
            .sprite = _state ? EnableSprite : DisableSprite;

        ButtonClicked?.Invoke(_state);
    }
}