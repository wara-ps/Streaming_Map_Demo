using UnityEngine;
using UnityEngine.UI;

public class NavbarMenu : MonoBehaviour
{
    [SerializeField]
    private Button _toggle = null;

    [SerializeField]
    private RectTransform _navbar = null;

    private bool _visible = false;

    private void Start()
    {
        _toggle.onClick.RemoveAllListeners();
        _toggle.onClick.AddListener(() =>
        {
            _visible = !_visible;
            RefreshVisibility();
        });

        RefreshVisibility();
    }

    private void RefreshVisibility()
    {
        if(_visible)
        {
            var pos = _navbar.anchoredPosition;
            pos.y = 0;
            _navbar.anchoredPosition = pos;
        }
        else
        {
            var pos = _navbar.anchoredPosition;
            pos.y = _navbar.rect.height;
            _navbar.anchoredPosition = pos;
        }
    }
}
