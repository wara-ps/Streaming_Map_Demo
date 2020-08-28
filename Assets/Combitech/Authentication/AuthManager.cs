using Assets.ThirdParty.MSAL;
using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Combitech.Authentication
{
    public enum AuthStatus
    {
        Undefined,
        NotAuthenticated,
        InProgress,
        Authenticated
    }

    public delegate void AuthUpdate(AuthStatus status);

    public class AuthManager : MonoBehaviour
    {
        public AuthStatus AuthStatus { get; private set; }
        public IAccount Account { get; private set; }
        public string AccountEmail => Account.Username;
        public string AccessToken { get; private set; }

        public event AuthUpdate OnAuthUpdate;

        public static AuthManager Instance { get; private set; }

        private static IPublicClientApplication _client { get; set; }

        private static string _instance = "https://login.microsoftonline.com/";
        private static string _tenantId = "7948ab45-e924-4cdc-84cd-5b7e20bb5aaa";
        private static string _clientId = "6035e2ab-6c0b-4030-8a3b-67d15f2ba315";
        private static string[] _scopes = new string[] { "user.read", "openid", "api://consensus.waraps.org/consensus.access" };

        private string _verificationUrl = null;
        private string _verificationCode = null;
        private AuthStatus _prevStatus = AuthStatus.Undefined;

        static AuthManager()
        {
            _client = PublicClientApplicationBuilder.Create(_clientId)
                    .WithAuthority($"{_instance}{_tenantId}")
                    .WithDefaultRedirectUri()
                    .Build();
            TokenCacheHelper.EnableSerialization(_client.UserTokenCache);
        }

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        private void Start()
        {
            SignIn(true);
        }

        private void Update()
        {
            if (_verificationUrl != null && _verificationCode != null)
            {
                GUIUtility.systemCopyBuffer = _verificationCode;
                Application.OpenURL(_verificationUrl);

                _verificationUrl = null;
                _verificationCode = null;
            }

            if (AuthStatus != _prevStatus)
            {
                OnAuthUpdate?.Invoke(AuthStatus);
                _prevStatus = AuthStatus;
            }
        }

        public async void SignIn(bool silent = false)
        {
            if (AuthStatus == AuthStatus.Authenticated)
            {
                return;
            }

            AuthStatus = AuthStatus.InProgress;
            AuthenticationResult authResult = null;
            var accounts = await _client.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();

            try
            {
                authResult = await _client.AcquireTokenSilent(_scopes, firstAccount)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilent. 
                // This indicates you need to call AcquireTokenWithDeviceCode to acquire a token
                Debug.LogError(ex);

                if (silent)
                {
                    ResetAuthentication();
                    return;
                }

                try
                {
                    authResult = await _client.AcquireTokenWithDeviceCode(_scopes, result =>
                    {
                        Debug.Log(result.Message);

                        _verificationUrl = result.VerificationUrl;
                        _verificationCode = result.UserCode;

                        Debug.Log(_verificationUrl);
                        Debug.Log(_verificationCode);

                        return Task.FromResult(0);
                    }).ExecuteAsync();
                }
                catch (MsalException msalex)
                {
                    Debug.LogError(msalex);
                    ResetAuthentication();
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                ResetAuthentication();
                return;
            }

            Debug.Log(authResult.Account.Username);
            Debug.Log(authResult.AccessToken);

            SetAuthentication(authResult);
        }

        public async void SignOut()
        {
            if (Account != null)
            {
                await _client.RemoveAsync(Account);
            }

            ResetAuthentication();
        }

        private void ResetAuthentication()
        {
            AuthStatus = AuthStatus.NotAuthenticated;
            Account = null;
            AccessToken = null;
        }

        private void SetAuthentication(AuthenticationResult result)
        {
            AuthStatus = AuthStatus.Authenticated;
            Account = result.Account;
            AccessToken = result.AccessToken;
        }
    }
}
