using Assets.Combitech.UserControl;
using GizmoSDK.GizmoBase;
using Saab.Foundation.Map;
using Saab.Foundation.Unity.MapStreamer;
using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public class PlayerDebugInfo : BaseDebugInfo
    {
        private ISceneManagerCamera _player;
        private float _offset = 0;

        public override int SetRow(int row)
        {
            _offset = row * RowHeight;
            return row + 3;
        }

        private void Start()
        {
            _player = FindObjectOfType<WorldCameraController>();
        }

        private void OnGUI()
        {
            if (_player != default)
            {
                var row0 = _offset + 0 * RowHeight;
                var row1 = _offset + 1 * RowHeight;
                var row2 = _offset + 2 * RowHeight;

                GUI.Label(new Rect(0, row0, HeaderWidth, RowHeight), $"Camera pos:", HeaderStyle);
                GUI.Label(new Rect(0, row1, HeaderWidth, RowHeight), $"Camera alt:", HeaderStyle);
                var pos = MapControl.SystemMap.GlobalToLocal(_player.GlobalPosition);
                if (MapControl.SystemMap.GetLatPos(pos, out var latlng))
                {
                    GUI.Label(new Rect(HeaderWidth, row0, ValueWidth, RowHeight), $"{latlng}", ValueStyle);
                    if (MapControl.SystemMap.GetGroundPosition(_player.GlobalPosition + new Vec3(0, 10000, 0), new Vec3(0, -1, 0), out var ground))
                    {
                        var alt = pos.position.y - ground.position.y;
                        GUI.Label(new Rect(HeaderWidth, row1, ValueWidth, RowHeight), $"{alt:0.##} m", ValueStyle);
                    }
                }

                GUI.Label(new Rect(0, row2, HeaderWidth, RowHeight), $"Camera rot:", HeaderStyle);
                var rot = _player.Camera.transform.localEulerAngles;
                GUI.Label(new Rect(HeaderWidth, row2, ValueWidth, RowHeight), $"{rot} °", ValueStyle);
            }
        }
    }
}
