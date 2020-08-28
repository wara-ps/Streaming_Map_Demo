using GizmoSDK.Coordinate;
using Saab.Foundation.Map;
using Saab.Utility.Unity.NodeUtils;
using System;
using System.Linq;
using UnityEngine;
using Wasp.Consensus.Domain.Models;

namespace Assets.Combitech.Environment
{
    public static class WorldUtils
    {
        public static GameObject GetParentNode(Position latlng)
        {
            var latpos = new LatPos
            {
                Latitude = Math.PI * latlng.Latitude / 180,
                Longitude = Math.PI * latlng.Longitude / 180,
                Altitude = Math.PI * latlng.Altitude / 180
            };
            return GetParentNode(latpos);
        }

        public static GameObject GetParentNode(LatPos latlng)
        {
            if (!MapControl.SystemMap.GetPosition(latlng, out var pos))
            {
                return null;
            }

            if (!NodeUtils.FindGameObjects(pos.node.GetNativeReference(), out var list))
            {
                return null;
            }

            var parent = list.FirstOrDefault();
            return parent;
        }

        public static Vector3 GetLocalPosition(Position latlng)
        {
            var latpos = new LatPos
            {
                Latitude = Math.PI * latlng.Latitude / 180,
                Longitude = Math.PI * latlng.Longitude / 180,
                Altitude = Math.PI * latlng.Altitude / 180
            };
            return GetLocalPosition(latpos);
        }

        public static Vector3 GetLocalPosition(LatPos latlng)
        {
            if (!MapControl.SystemMap.GetPosition(latlng, out var lpos))
            {
                return Vector3.zero;
            }

            return new Vector3(lpos.LocalPosition.X, lpos.LocalPosition.Y, lpos.LocalPosition.Z);
        }
    }
}
