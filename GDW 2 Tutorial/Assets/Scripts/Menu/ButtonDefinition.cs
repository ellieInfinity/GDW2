using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonDefinition : MonoBehaviour
{
    public bool _animated = false;
    public Color _unselectedtint = Color.grey;
    public Color _selectedtint = Color.white;
    public bool _selected = false;
    private Button _button;
    private Image _image;

    public AudioClip _swapToSFX;
    public AudioClip _confirmSFX;
    public float _confirmTime;
    private Animator _animator;

    private bool _disableControls = false;
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        _animated = TryGetComponent<Animator> (out _animator);

        if (!_animated)
        {

            if (_selected)
            {
                _image.color = _selectedtint;
            }
            else
            {
                _image.color = _unselectedtint;
            }
        }
    }

    public void SwappedTo()
    {
        _selected = true;

        if (_swapToSFX != null)
        {
            AudioSource.PlayClipAtPoint (_swapToSFX, Vector3.zero);
        }

        if (_animated)
        {
            _animator.SetBool("Selected", _selected);
        }
        else
        {
            _image.color = _selectedtint;
        }
    }

    public void SwappedOff()
    {
        _selected = false;

        if (_animated)
        {
            _animator.SetBool("Selected", _selected);
        }
        else
        {
            _image.color = _unselectedtint;
        }
    }

    public IEnumerator ClickButton()
    {
        if (! _disableControls)
        {
            _disableControls = true;

            if (_confirmSFX != null)
        {
            AudioSource.PlayClipAtPoint (_confirmSFX, Vector3.zero);
        }

            yield return new WaitForSeconds(_confirmTime);

            _button.onClick.Invoke();
            _disableControls = false;
        }

    }

    public bool GetDisableControls()
    {
        return _disableControls;
    }
}
