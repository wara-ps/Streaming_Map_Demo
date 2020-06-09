using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public class FpsDebugInfo : BaseDebugInfo
    {
        private double _frameDurationTime = 0;
        private double _frameTime = 0;
        private double _fps = 0;
        private float _offset;

        private void Update()
        {
            double time = GizmoSDK.GizmoBase.Time.SystemSeconds;

            if (_frameTime > 0)
                _frameDurationTime = 0.99 * _frameDurationTime + 0.01 * (time - _frameTime);

            _frameTime = time;
            _fps = 1.0 / _frameDurationTime;
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(0, _offset, HeaderWidth, RowHeight), $"FPS:", HeaderStyle);
            GUI.Label(new Rect(HeaderWidth, _offset, ValueWidth, RowHeight), $"{Math.Round(_fps)}", ValueStyle);
        }

        public override int SetRow(int row)
        {
            _offset = row * RowHeight;
            return row + 1;
        }
    }
}
