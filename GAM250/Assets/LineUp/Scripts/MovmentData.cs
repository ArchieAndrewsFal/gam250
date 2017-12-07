using UnityEngine;
using System.Collections.Generic;

namespace LineUp
{
    [System.Serializable]
    public class MovmentData
    {
        public int id;
        public Transform transformToTrack;
        public List<Vector3> movmentData;
    }
}
