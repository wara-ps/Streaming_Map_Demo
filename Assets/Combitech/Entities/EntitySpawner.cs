using GizmoSDK.GizmoBase;
using UnityEngine;

namespace Assets.Combitech.Entities
{
    public class EntitySpawner : MonoBehaviour
    {
        public GameObject SpawnEntity(GameObject parent, Vec3D pos, PrimitiveType type)
        {
            var entity = CreatePrimitive(parent, pos, type);
            Debug.Log($"Create primitive {type} at local position {pos}");
            return entity;
        }

        private GameObject CreatePrimitive(GameObject parent, Vec3D pos, PrimitiveType type)
        {
            var sphere = GameObject.CreatePrimitive(type);

            sphere.transform.parent = parent.transform;
            sphere.transform.transform.localPosition = new Vector3((float)pos.x, (float)pos.y, (float)pos.z);
            sphere.transform.localScale = new Vector3(10, 10, 10);

            var renderer = sphere.GetComponent<MeshRenderer>();
            renderer.material = new Material(Shader.Find("Legacy Shaders/Diffuse"));

            return sphere;
        }
    }
}
