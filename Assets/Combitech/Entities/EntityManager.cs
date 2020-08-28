using Assets.Combitech.Consensus;
using Assets.Combitech.Environment;
using GizmoSDK.Coordinate;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wasp.Consensus.Domain.Models;
using Wasp.Consensus.Shared.Serialization;

namespace Assets.Combitech.Entities
{
    public class EntityManager : MonoBehaviour
    {
        public readonly List<Entity> Entities = new List<Entity>();

        public readonly List<SelectableObject> SelectableObjects = new List<SelectableObject>();

        public Entity Selected => Entities.FirstOrDefault(x => x.Selected);

        private ConsensusApi _api => ConsensusApi.Instance;

        private EntitySpawner _spawner;

        private readonly string _typename = SerializationUtils.GetTypeName(typeof(Unit));

        private void Start()
        {
            _spawner = FindObjectOfType<EntitySpawner>();

            _api.OnSelectionUpdate += Api_OnSelectionUpdate;
            //_api.OnFocusUpdate += Api_OnFocusUpdate;

            _api.OnFlush += Api_OnFlush;

            _api.OnUnitCreated += Api_OnUnitCreated;
            _api.OnUnitUpdated += Api_OnUnitUpdated;
            _api.OnUnitRemoved += Api_OnUnitRemoved;

            SelectableObjects.AddRange(FindObjectsOfType<SelectableObject>());
        }

        private void Api_OnFlush()
        {
            Debug.Log("Flushing entities");
            foreach (var entity in Entities)
            {
                Destroy(entity.gameObject);
            }

            Entities.Clear();
            SelectableObjects.Clear();
        }

        private void Api_OnSelectionUpdate(Selection selection)
        {
            Debug.Log(_typename);
            var entry = selection.SelectedEntities.FirstOrDefault(x => x.Type == _typename);
            if (entry == default)
            {
                SelectableObjects.ForEach(x => x.Selected = false);
                return;
            }

            var entity = Entities.FirstOrDefault(x => x.Data.Id == entry.Id);
            SelectableObjects.ForEach(x => x.Selected = x.gameObject == entity.gameObject);
        }

        private void Api_OnFocusUpdate(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }

        private void Api_OnUnitCreated(Unit unit)
        {
            var parent = WorldUtils.GetParentNode(unit.Position);
            var position = WorldUtils.GetLocalPosition(unit.Position);

            Debug.Log($"{unit.Name} ({unit.Position.Latitude}, {unit.Position.Longitude}) {position}");

            var sphere = _spawner.SpawnEntity(parent, position, PrimitiveType.Sphere);
            sphere.name = $"Sphere {unit.Name} ({unit.Position})";

            var selectable = sphere.AddComponent<SelectableObject>();
            SelectableObjects.Add(selectable);

            var entity = sphere.AddComponent<Entity>();
            entity.Data = unit;
            Entities.Add(entity);
        }

        private void Api_OnUnitUpdated(Unit unit)
        {
            var entity = Entities.FirstOrDefault(x => x.Data.Id == unit.Id);
            if (entity == default)
            {
                return;
            }

            var parent = WorldUtils.GetParentNode(unit.Position);
            var position = WorldUtils.GetLocalPosition(unit.Position);

            entity.transform.SetParent(parent.transform);
            entity.transform.localPosition = position;
        }

        private void Api_OnUnitRemoved(Unit entity)
        {
            //throw new System.NotImplementedException();
        }

        private void SelectObject(SelectableObject selected)
        {
            var previous = SelectableObjects.FirstOrDefault(x => x.Selected);
            if (previous == selected)
            {
                previous.Selected = !previous.Selected;
            }
            else
            {
                SelectableObjects.ForEach(x => x.Selected = x == selected);
            }
        }

    }
}
