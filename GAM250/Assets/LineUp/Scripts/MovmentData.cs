using UnityEngine;
using System.Collections.Generic;

namespace LineUp
{
    [System.Serializable]
    public class MovmentData
    {
        public float id;
        public Transform transformToTrack;
        public string movementString;
        public Vector3 lastRecordedPosition;
    }
}
