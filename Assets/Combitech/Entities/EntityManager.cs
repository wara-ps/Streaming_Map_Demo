using Boo.Lang;
using UnityEngine;

namespace Assets.Combitech.Entities
{
    public class EntityManager : MonoBehaviour
    {
        public List<Entity> Entities { get; } = new List<Entity>();

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
        }

        public void UpdateEntity(Entity entity)
        {
            //
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
        }
    }
}
