using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public class ShortcutController : MonoBehaviour
    {
        private AppConfiguration _config;

        private void Start()
        {
            _config = FindObjectOfType<AppConfiguration>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                _config.Settings.App.Fullscreen = !_config.Settings.App.Fullscreen;
            }
        }
    }
}
