using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using GizmoSDK.GizmoBase;

namespace Assets.Combitech
{
    public class AppConfiguration : MonoBehaviour
    {
        public ConfigRoot Settings { get; private set; }

        public ConfigMapItem SelectedMap { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            var path = Path.Combine(Application.streamingAssetsPath, "config.json");
            var json = File.ReadAllText(path, Encoding.UTF8);
            Settings = JsonConvert.DeserializeObject<ConfigRoot>(json);

            Debug.Log(Settings.Maps.First().Id);
        }

        public void SelectMap(Guid id)
        {
            SelectedMap = Settings.Maps.FirstOrDefault(x => x.Id == id);
        }

        public class ConfigRoot
        {
            public List<ConfigMapItem> Maps { get; set; }
        }

        public class ConfigMapItem
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
            public Vec3D StartPosition { get; set; }
        }
    }
}
