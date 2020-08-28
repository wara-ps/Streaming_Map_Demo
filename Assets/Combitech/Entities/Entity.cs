using UnityEngine;
using Wasp.Consensus.Domain.Models;

namespace Assets.Combitech.Entities
{
    public class Entity : MonoBehaviour
    {
        public BaseEntity Data { get; set; }

        private SelectableObject _selectable;

        public bool Selected => _selectable.Selected;

        private void Start()
        {
            _selectable = GetComponent<SelectableObject>();
        }
    }
}
