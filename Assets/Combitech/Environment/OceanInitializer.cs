using Assets.Combitech.UserControl;
using Crest;
using GizmoSDK.GizmoBase;
using Saab.Foundation.Map;
using Saab.Foundation.Unity.MapStreamer;
using Saab.Unity.Extensions;
using Saab.Utility.Unity.NodeUtils;
using System.Linq;
using UnityEngine;
using Transform = UnityEngine.Transform;

namespace Assets.Combitech.Environment
{
    public class OceanInitializer : MonoBehaviour
    {
        private ISceneManagerCamera _player;
        private Material _material;
        private Transform _parent;

        private void Start()
        {
            _player = FindObjectOfType<WorldCameraController>();
            _material = GetComponent<OceanRenderer>().OceanMaterial;
            _material.SetVector("_WorldOffset", Vector3.zero);
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

            // Does not work, why?
            // _material.SetVector("_WorldOffset", transform.localPosition);
        }
    }
}
