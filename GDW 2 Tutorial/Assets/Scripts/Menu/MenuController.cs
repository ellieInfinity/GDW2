using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject _activeMenu;
    public AudioSource _backgroundAudio;
    public List<KeyCode> _increaseVert;
    public List<KeyCode> _decreaseVert;
    public List<KeyCode> _increaseHoriz;
    public List<KeyCode> _decreaseHoriz;
    public List<KeyCode> _confirmButton;

    private MenuDefinition _activeMenuDefinition;
    private int _activeButton = 0;

    public void Start()
    {
        UpdateActiveMenuPosition();
    }

    public void Update()
    {
        switch (_activeMenuDefinition.GetMenuType())
        {
            case menutype.HORIZONTAL:
                MenuInput(_increaseHoriz, _decreaseHoriz);
                break;
            case menutype.VERTICAL:
                MenuInput(_increaseVert, _decreaseVert);
                break;
        }

    }

    private void MenuInput(List<KeyCode> increase, List<KeyCode> decrease)
    {
        int newActive = _activeButton;

        for(int i = 0; i < increase.Count; i++)
        {
            if (Input.GetKeyDown(increase[i]))
            {
                newActive = SwitchCurrentButton(1);
            }
        }

        for (int i = 0; i < decrease.Count; i++)
        {
            if (Input.GetKeyDown(decrease[i]))
            {
                newActive = SwitchCurrentButton(-1);
            }
        }

        for (int i = 0; i < _confirmButton.Count; i++)
        {
            if (Input.GetKeyDown(_confirmButton[i]))
            {
                ClickCurrentButton();
            }
        }

        _activeButton = newActive;
    }

    private int SwitchCurrentButton(int increment)
    {
        if (!_activeMenuDefinition.getButtonDefinitions()[_activeButton].GetDisableControls())
        {
            int newActive = Utility.WrapAround(_activeMenuDefinition.GetButtonCount(), _activeButton, increment);
            _activeMenuDefinition.getButtonDefinitions()[_activeButton].SwappedOff();
            _activeMenuDefinition.getButtonDefinitions() [newActive].SwappedTo();

            return newActive;
        }
        return _activeButton;
    }

    private void ClickCurrentButton()
    {
        if (!_activeMenuDefinition.getButtonDefinitions()[_activeButton].GetDisableControls())
        {
            StartCoroutine(_activeMenuDefinition.getButtonDefinitions()[_activeButton].ClickButton());
        }

    }

    public void UpdateActiveMenuPosition()
    {
        _activeMenuDefinition = _activeMenu.GetComponent<MenuDefinition>();

        if (_activeMenuDefinition._menuMusic != null)
        {
            _backgroundAudio.clip = _activeMenuDefinition._menuMusic;
            _backgroundAudio.Play();
        }

        else if (!_activeMenuDefinition._continuePrevMusic)
        {
            _backgroundAudio.Stop();
        }
    }

    public void SetActiveMenu(GameObject activeMenu)
    {
        _activeMenu = activeMenu;
        UpdateActiveMenuPosition();
    }
}
