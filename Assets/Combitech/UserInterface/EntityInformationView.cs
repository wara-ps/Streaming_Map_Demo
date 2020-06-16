using Assets.Combitech.Entities;
using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public class EntityInformationView : MonoBehaviour
    {
        [SerializeField]
        private SidePanel _container;

        private EntityManager _manager;
        private Entity _selected;

        private void Start()
        {
            _manager = FindObjectOfType<EntityManager>();
        }

        private void Update()
        {
            if (_selected != _manager.Selected)
            {
                _selected = _manager.Selected;
                _container.Toggle(_selected != null);
            }
        }
    }
}
