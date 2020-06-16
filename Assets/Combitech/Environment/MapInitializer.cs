using Assets.Combitech.UserControl;
using GizmoSDK.GizmoBase;
using Saab.Foundation.Unity.MapStreamer;
using UnityEngine;

namespace Assets.Combitech.Environment
{
    public class MapInitializer : MonoBehaviour
    {
        protected void Start()
        {
            Platform.Initialize();
            Message.OnMessage += Message_OnMessage;

            Message.SetMessageLevel(MessageLevel.PERF_DEBUG);

            // Initialize application ragistry
            KeyDatabase.SetDefaultRegistry($"/data/data/{Application.identifier}/files/gizmosdk.reg");

            // Set local xml config
            KeyDatabase.SetLocalRegistry("config.xml");

            // Set up scene manager camera
            var ctl = FindObjectOfType<WorldCameraController>();
            var mgr = GetComponent<SceneManager>();
            mgr.SceneManagerCamera = ctl;

            var config = FindObjectOfType<AppConfiguration>();
            if (config)
            {
                ctl.X = config.SelectedMap.StartPosition.x;
                ctl.Y = config.SelectedMap.StartPosition.y;
                ctl.Z = config.SelectedMap.StartPosition.z;
                mgr.MapUrl = config.SelectedMap.Url;
            }
        }

        private void Message_OnMessage(string sender, MessageLevel level, string message)
        {
            // Just to route some messages from Gizmo to managed unity

            switch (level & MessageLevel.LEVEL_MASK)
            {
                case MessageLevel.MEM_DEBUG:
                case MessageLevel.PERF_DEBUG:
                case MessageLevel.DEBUG:
                case MessageLevel.TRACE_DEBUG:
                    Debug.Log(message);
                    break;

                case MessageLevel.NOTICE:
                    Debug.Log(message);
                    break;

                case MessageLevel.WARNING:
                    Debug.LogWarning(message);
                    break;

                case MessageLevel.FATAL:
                    Debug.LogError(message);
                    break;

                case MessageLevel.ASSERT:
                    Debug.LogAssertion(message);
                    break;

                case MessageLevel.ALWAYS:
                    Debug.Log(message);
                    break;
            }
        }
    }
}
