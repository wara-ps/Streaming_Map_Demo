using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Assets.Combitech.AppConfiguration;

namespace Assets.Combitech.UserInterface
{
    public class MapSelectMenu : MonoBehaviour
    {
        public Transform ContentParent;
        public LoadingIndicator LoadingIndicator;

        private AppConfiguration _config;
        private List<GameObject> _items = new List<GameObject>();

        private void Start()
        {
            _config = FindObjectOfType<AppConfiguration>();

            foreach (var map in _config.Settings.Maps)
            {
                var listitem = CreateMenuItem(map.Name, map.Url, () => SelectMap(map));
                listitem.transform.SetParent(ContentParent);
                _items.Add(listitem);
            }

            LoadingIndicator.gameObject.SetActive(false);
        }

        private GameObject CreateMenuItem(string title, string url, UnityAction action)
        {
            var g = new GameObject();
            g.name = title;

            var layout = g.AddComponent<LayoutElement>();

            var rect = g.GetComponent<RectTransform>();
            rect.sizeDelta = 60 * Vector2.up;

            var header = new GameObject().AddComponent<Text>();
            header.transform.SetParent(g.transform);
            header.text = title;
            header.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            header.fontSize = 24;
            var hrect = header.GetComponent<RectTransform>();
            hrect.anchorMin = new Vector2(0, 0.5f);
            hrect.anchorMax = new Vector2(1, 1);
            hrect.sizeDelta = Vector2.zero;

            var subheader = new GameObject().AddComponent<Text>();
            subheader.transform.SetParent(g.transform);
            subheader.text = url;
            subheader.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            subheader.resizeTextForBestFit = true;
            subheader.resizeTextMaxSize = 18;
            var srect = subheader.GetComponent<RectTransform>();
            srect.anchorMin = new Vector2(0, 0);
            srect.anchorMax = new Vector2(1, 0.5f);
            srect.sizeDelta = Vector2.zero;

            var button = g.AddComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
            button.targetGraphic = header;

            return g;
        }

        private void SelectMap(ConfigMapItem map)
        {
            _config.SelectMap(map.Id);
            StartCoroutine(LoadWorldSceneAsync());
        }

        private IEnumerator LoadWorldSceneAsync()
        {
            LoadingIndicator.gameObject.SetActive(true);
            _items.ForEach(x => x.GetComponent<Button>().interactable = false);

            var load = SceneManager.LoadSceneAsync("WorldScene");
            while (!load.isDone)
            {
                yield return null;
            }
        }
    }
}
