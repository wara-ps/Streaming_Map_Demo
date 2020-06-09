using Assets.Combitech.Entities;
using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public class EntityDebugInfo : BaseDebugInfo
    {
        private EntityManager _manager;
        private float _offset = 0;

        private void Start()
        {
            _manager = FindObjectOfType<EntityManager>();
        }

        private void OnGUI()
        {
            if (_manager)
            {
                GUI.Label(new Rect(0, _offset, HeaderWidth, RowHeight), $"Entity count:", HeaderStyle);
                GUI.Label(new Rect(HeaderWidth, _offset, ValueWidth, RowHeight), $"{_manager.Entities.Count}", ValueStyle);
            }
        }

        public override int SetRow(int row)
        {
            _offset = row * RowHeight;
            return row + 1;
        }
    }
}
