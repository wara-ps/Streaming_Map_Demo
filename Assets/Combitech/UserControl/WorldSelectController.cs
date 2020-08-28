using Assets.Combitech.Consensus;
using Assets.Combitech.Entities;
using Saab.Foundation.Map;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Combitech.UserControl
{
    public class WorldSelectController : MonoBehaviour
    {
        private EntityManager _manager;
        private EntitySpawner _spawner;

        private void Start()
        {
            _manager = FindObjectOfType<EntityManager>();
            _spawner = FindObjectOfType<EntitySpawner>();
        }

        private async void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (PointerIsOverUi())
                    return;

                if (await CheckGameObjectClick())
                {
                    return;
                }

                if (await CheckGroundClick())
                {
                    return;
                }
            }
        }

        private async Task<bool> CheckGameObjectClick()
        {
            var camera = GetComponent<Camera>();
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var info))
            {
                Debug.Log($"Clicked on object: {info.transform.name}");

                var focused = info.transform.GetComponentInParent<SelectableObject>();
                var entity = focused.GetComponent<Entity>();
                await ConsensusApi.Instance.Select(entity.Data);
                //if (Input.GetKey(KeyCode.LeftShift))
                //{
                //    _manager.RemoveEntity(focused.GetComponent<Entity>());
                //    Destroy(focused.gameObject);
                //}
                //else
                //{
                //SelectObject(focused);
                //}

                return true;
            }

            return false;
        }

        private async Task<bool> CheckGroundClick()
        {
            if (GetClickedGroundPosition(out var pos) && pos.clamp_result != GizmoSDK.Gizmo3D.IntersectQuery.NULL)
            {
                //MapControl.SystemMap.LocalToWorld(pos, out LatPos latlng);
                //if (NodeUtils.FindGameObjects(pos.node.GetNativeReference(), out var list))
                //{
                //    var parent = list.FirstOrDefault();
                //    if (parent)
                //    {
                //        var localpos = new Vector3((float)pos.position.x, (float)pos.position.y, (float)pos.position.z);
                //        var sphere = _spawner.SpawnEntity(parent, localpos, PrimitiveType.Sphere);
                //        sphere.name = $"Sphere ({latlng})";

                //        var selectable = sphere.AddComponent<SelectableObject>();
                //        SelectableObjects.Add(selectable);

                //        var entity = sphere.AddComponent<Entity>();
                //        _manager.AddEntity(entity);
                //    }
                //}
                await ConsensusApi.Instance.Select(null);

                return true;
            }

            return false;
        }

        private bool GetClickedGroundPosition(out MapPos pos)
        {
            int x = (int)Input.mousePosition.x;
            int y = (int)(Screen.height - Input.mousePosition.y);
            uint w = (uint)Screen.width;
            uint h = (uint)Screen.height;

            return MapControl.SystemMap.GetScreenGroundPosition(x, y, w, h, out pos);
        }

        private static bool PointerIsOverUi()
        {
            if (Input.touchCount > 0)
                return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
