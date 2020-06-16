using System.Collections;
using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public class SidePanel : MonoBehaviour
    {
        private RectTransform _rect;
        private Coroutine _transition;
        private float _target = 0;

        private void Start()
        {
            _rect = GetComponent<RectTransform>();
            Hide();
        }

        public void Show()
        {
            Toggle(true);
        }

        public void Hide()
        {
            Toggle(false);
        }

        public void Toggle(bool show)
        {
            _target = show ? 0 : _rect.rect.width;
            if (_transition == null)
            {
                _transition = StartCoroutine(Animate());
            }
        }

        private IEnumerator Animate()
        {
            var pos = _rect.anchoredPosition;

            while (Mathf.Abs(pos.x - _target) > 0.001f)
            {
                pos.x = Mathf.Lerp(pos.x, _target, 10 * Time.deltaTime);
                _rect.anchoredPosition = pos;
                yield return null;
            }

            pos.x = _target;
            _rect.anchoredPosition = pos;
            _transition = null;
        }
    }
}
