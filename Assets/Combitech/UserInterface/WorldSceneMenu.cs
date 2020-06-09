using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Combitech.UserInterface
{
    public class WorldSceneMenu : MonoBehaviour
    {
        public Button BackButton;
        public LoadingIndicator LoadingIndicator;

        private void Start()
        {
            LoadingIndicator.gameObject.SetActive(false);

            BackButton.onClick.RemoveAllListeners();
            BackButton.onClick.AddListener(() =>
            {
                var config = FindObjectOfType<AppConfiguration>();
                if (config)
                {
                    Destroy(config.gameObject);
                }

                StartCoroutine(LoadLobbySceneAsync());
            });
        }

        private IEnumerator LoadLobbySceneAsync()
        {
            LoadingIndicator.gameObject.SetActive(true);
            BackButton.interactable = false;

            var load = SceneManager.LoadSceneAsync("LobbyScene");
            while (!load.isDone)
            {
                yield return null;
            }
        }
    }
}
