using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public class FullscreenManager : MonoBehaviour
    {
        private bool _fullscreen { get; set; }

        private Resolution _resolution = new Resolution();
        private AppConfiguration _config;

        private void Start()
        {
            _config = FindObjectOfType<AppConfiguration>();
            _fullscreen = !_config.Settings.App.Fullscreen; // trigger first update
            _resolution.width = Screen.width;
            _resolution.height = Screen.height;
        }

        private void Update()
        {
            if (_fullscreen != _config.Settings.App.Fullscreen)
            {
                if (!_fullscreen)
                {
                    _resolution.width = Screen.width;
                    _resolution.height = Screen.height;
                }

                int w = _config.Settings.App.Fullscreen ? Screen.currentResolution.width : _resolution.width;
                int h = _config.Settings.App.Fullscreen ? Screen.currentResolution.height : _resolution.height;
                Screen.SetResolution(w, h, _config.Settings.App.Fullscreen);
                _fullscreen = _config.Settings.App.Fullscreen;
            }
        }
    }
}
