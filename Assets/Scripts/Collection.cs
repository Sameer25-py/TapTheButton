using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviour
{
    public GameManager GameManager;
    public Image       DisableMask;
    public Button      CollectionScrollButton;
    public TMP_Text    ScrollNoText;
    public GameObject  UnfoldScroll;
    public TMP_Text    ScrollText;

    private int  _availableScroll;
    private bool _isScrollPressed;
    

    private void OnEnable()
    {
        UpdateScrollCollectionUI();
    }

    public void MoveNextScroll()
    {
        if (_availableScroll + 1 < GameManager.ScrollPrompts.Count)
        {
            _availableScroll += 1;
            UpdateScrollCollectionUI();
        }
    }

    public void MovePreviousScroll()
    {
        if (_availableScroll - 1 >= 0)
        {
            _availableScroll -= 1;
            UpdateScrollCollectionUI();
        }
    }

    private void UpdateScrollCollectionUI()
    {   
        UnfoldScroll.SetActive(false);
        ScrollNoText.text = (_availableScroll + 1).ToString();
        if (_availableScroll < GameManager.LastUnlockedScroll)
        {
            CollectionScrollButton.interactable = true;
            DisableMask.enabled                 = false;
        }
        else
        {
            CollectionScrollButton.interactable = false;
            DisableMask.enabled                 = true;
        }
    }

    public void ScrollButton()
    {
        _isScrollPressed = true;
        ScrollText.text  = GameManager.ScrollPrompts[_availableScroll];
        UnfoldScroll.SetActive(true);
        
    }

    public void OkButton()
    {
        if (_isScrollPressed)
        {
            _isScrollPressed = false;
            UnfoldScroll.SetActive(false);
        }
        else
        {
            GameManager.HomeButton();
        }
    }   
}
