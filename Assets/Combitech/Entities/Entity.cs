using UnityEngine;

namespace Assets.Combitech.Entities
{
    public class Entity : MonoBehaviour
    {
        private SelectableObject _selectable;

        public bool Selected => _selectable.Selected;

        private void Start()
        {
            _selectable = GetComponent<SelectableObject>();
        }
    }
}
