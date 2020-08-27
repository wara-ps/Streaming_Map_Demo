using Assets.Combitech.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class AuthMenu : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private Text _text;

    private void Start()
    {
        AuthManager.Instance.OnAuthUpdate += OnAuthUpdate;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            if (AuthManager.Instance.AuthStatus == AuthStatus.Authenticated)
            {
                AuthManager.Instance.SignOut();
            }
            else
            {
                AuthManager.Instance.SignIn();
            }
        });

        OnAuthUpdate(AuthManager.Instance.AuthStatus);
    }

    private void OnDestroy()
    {
        AuthManager.Instance.OnAuthUpdate -= OnAuthUpdate;
    }

    private void OnAuthUpdate(AuthStatus status)
    {
        if (status == AuthStatus.Authenticated)
        {
            _text.text = AuthManager.Instance.AccountEmail;
            _button.interactable = true;
            _button.GetComponentInChildren<Text>().text = "sign out";
        }
        else if (status == AuthStatus.InProgress)
        {
            _text.text = "signing in...";
            _button.interactable = false;
            _button.GetComponentInChildren<Text>().text = "sign in";
        }
        else
        {
            _text.text = "not signed in";
            _button.interactable = true;
            _button.GetComponentInChildren<Text>().text = "sign in";
        }
    }
}
