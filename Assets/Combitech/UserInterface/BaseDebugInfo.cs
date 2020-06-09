using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public abstract class BaseDebugInfo : MonoBehaviour
    {
        protected GUIStyle HeaderStyle;
        protected GUIStyle ValueStyle;

        protected readonly float HeaderWidth = 100;
        protected readonly float ValueWidth = 160;
        protected readonly float RowHeight = 20;

        private void Awake()
        {
            HeaderStyle = new GUIStyle(GetDefaultStyle())
            {
                alignment = TextAnchor.MiddleLeft
            };
            ValueStyle = new GUIStyle(GetDefaultStyle())
            {
                alignment = TextAnchor.MiddleRight
            };
        }

        private static GUIStyle GetDefaultStyle()
        {
            var style = new GUIStyle
            {
                alignment = TextAnchor.MiddleRight,
                padding = new RectOffset(8, 8, 0, 0)
            };
            style.normal.textColor = Color.magenta;

            return style;
        }

        public abstract int SetRow(int row);
    }
}
