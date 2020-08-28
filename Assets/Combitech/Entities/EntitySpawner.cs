using UnityEngine;

namespace Assets.Combitech.Entities
{
    public class EntitySpawner : MonoBehaviour
    {
        public GameObject SpawnEntity(GameObject parent, Vector3 pos, PrimitiveType type)
        {
            var entity = CreatePrimitive(parent, pos, type);
            Debug.Log($"Create primitive {type} at local position {pos}");
            return entity;
        }

        private GameObject CreatePrimitive(GameObject parent, Vector3 pos, PrimitiveType type)
        {
            var sphere = GameObject.CreatePrimitive(type);

            sphere.transform.SetParent(parent.transform);
            sphere.transform.localPosition = pos;
            sphere.transform.localScale = new Vector3(10, 10, 10);

            var renderer = sphere.GetComponent<MeshRenderer>();
            renderer.material = new Material(Shader.Find("Legacy Shaders/Diffuse"));

            return sphere;
        }
    }
}
