using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public class LoadingIndicator : MonoBehaviour
    {
        private RectTransform _child;

        private void Start()
        {
            _child = transform.GetChild(0).GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_child && _child.gameObject.activeInHierarchy)
            {
                _child.Rotate(0, 0, 400 * Time.deltaTime);
            }
        }
    }
}
