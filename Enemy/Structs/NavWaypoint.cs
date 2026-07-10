using System;
using UnityEngine;

namespace Enemy.Structs
{
    [System.Serializable]
    public struct NavWaypoint : IEquatable<NavWaypoint>
    {
        public bool isChecked;
        public Vector3 position;

        public NavWaypoint(Vector3 pos)
        {
            isChecked = false;
            position = pos;
        }
        public bool Equals(NavWaypoint other)
        {
            return position.Equals(other.position);
        }
        public override bool Equals(object obj)
        {
            return obj is NavWaypoint p && Equals(p);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(position);
        }
        public static bool operator ==(NavWaypoint left, NavWaypoint right) => left.Equals(right);
        public static bool operator !=(NavWaypoint left, NavWaypoint right) => !left.Equals(right);
    }
}