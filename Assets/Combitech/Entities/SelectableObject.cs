using cakeslice;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Combitech.Entities
{
    public class SelectableObject : MonoBehaviour
    {
        public bool Selected { get; set; }

        private readonly List<Outline> Outlines = new List<Outline>();

        private void Start()
        {
            var meshes = GetComponentsInChildren<MeshRenderer>().ToList();
            Outlines.AddRange(meshes.Select(x => x.gameObject.AddComponent<Outline>()));
            Outlines.ForEach(x => x.enabled = false);
        }

        private void Update()
        {
            Outlines.ForEach(x => x.enabled = Selected);
        }
    }
}
