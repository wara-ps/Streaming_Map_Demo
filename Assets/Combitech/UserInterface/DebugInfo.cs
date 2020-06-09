using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Combitech.UserInterface
{
    public class DebugInfo : MonoBehaviour
    {
        public bool Enabled;

        private List<BaseDebugInfo> _infos = new List<BaseDebugInfo>();

        private void Start()
        {
            _infos = GetComponents<BaseDebugInfo>().ToList();
            int row = 0;
            foreach (var info in _infos)
            {
                info.enabled = Enabled;
                row = info.SetRow(row);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                Enabled = !Enabled;
                _infos.ForEach(x => x.enabled = Enabled);
            }
        }
    }
}
