using Assets.Combitech.Entities;
using GizmoSDK.Coordinate;
using Saab.Foundation.Map;
using Saab.Utility.Unity.NodeUtils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Combitech.UserControl
{
    public class WorldSelectController : MonoBehaviour
    {
        private readonly List<SelectableObject> SelectableObjects = new List<SelectableObject>();
        private EntityManager _manager;
        private EntitySpawner _spawner;

        private void Start()
        {
            SelectableObjects.AddRange(FindObjectsOfType<SelectableObject>());
            _manager = FindObjectOfType<EntityManager>();
            _spawner = FindObjectOfType<EntitySpawner>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (PointerIsOverUi())
                    return;

                if (CheckGameObjectClick())
                {
                    return;
                }

                SelectableObjects.ForEach(x => x.Selected = false);

                if (CheckGroundClick())
                {
                    return;
                }
            }
        }

        private bool CheckGameObjectClick()
        {
            var camera = GetComponent<Camera>();
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var info))
            {
                Debug.Log($"Clicked on object: {info.transform.name}");

                var focused = info.transform.GetComponentInParent<SelectableObject>();
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _manager.RemoveEntity(focused.GetComponent<Entity>());
                    Destroy(focused.gameObject);
                }
                else
                {
                    SelectObject(focused);
                }

                return true;
            }

            return false;
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

        private bool CheckGroundClick()
        {
            if (GetClickedGroundPosition(out var pos) && pos.clamp_result != GizmoSDK.Gizmo3D.IntersectQuery.NULL)
            {
                MapControl.SystemMap.LocalToWorld(pos, out LatPos latlng);
                if (NodeUtils.FindGameObjects(pos.node.GetNativeReference(), out var list))
                {
                    var parent = list.FirstOrDefault();
                    if (parent)
                    {
                        var sphere = _spawner.SpawnEntity(parent, pos.position, PrimitiveType.Sphere);
                        sphere.name = $"Sphere ({latlng})";

                        var selectable = sphere.AddComponent<SelectableObject>();
                        SelectableObjects.Add(selectable);

                        var entity = sphere.AddComponent<Entity>();
                        _manager.AddEntity(entity);
                    }
                }

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
