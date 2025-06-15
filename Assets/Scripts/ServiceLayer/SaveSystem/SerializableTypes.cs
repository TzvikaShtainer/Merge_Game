
using UnityEngine;

namespace ServiceLayer.SaveSystem
{
    public class SerializableTypes
    {
        [System.Serializable]
        public struct SerializableVector2
        {
            public float x, y;

            public Vector2 ToVector2() => new(x, y);
            public static SerializableVector2 From(Vector2 v) => new() { x = v.x, y = v.y };
        }

        [System.Serializable]
        public struct SerializableQuaternion
        {
            public float x, y, z, w;

            public Quaternion ToQuaternion() => new(x, y, z, w);
            public static SerializableQuaternion From(Quaternion q) => new() { x = q.x, y = q.y, z = q.z, w = q.w };
        }
    }
}