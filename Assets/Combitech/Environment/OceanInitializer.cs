using Assets.Combitech.UserControl;
using Saab.Foundation.Map;
using Saab.Foundation.Unity.MapStreamer;
using Saab.Unity.Extensions;
using Saab.Utility.Unity.NodeUtils;
using System.Linq;
using UnityEngine;

namespace Assets.Combitech.Environment
{
    public class OceanInitializer : MonoBehaviour
    {
        private ISceneManagerCamera _player;

        private void Start()
        {
            _player = FindObjectOfType<WorldCameraController>();
        }

        private void Update()
        {
            SetParentTransform();
        }

        private void SetParentTransform()
        {
            var gpos = _player.GlobalPosition;
            gpos.y = 0;
            var lpos = MapControl.SystemMap.GlobalToLocal(gpos);
            if (NodeUtils.FindGameObjects(lpos.node.GetNativeReference(), out var list))
            {
                var parent = list.FirstOrDefault();
                if (parent)
                {
                    transform.SetParent(parent.transform);
                    transform.localPosition = lpos.position.ToVector3();
                }
            }
        }
    }
}
