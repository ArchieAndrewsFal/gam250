using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUp
{
    public class LineUpSettings : ScriptableObject
    {
        [Tooltip("The time between position and null checks in the manager")]
        public float cycleTime = 0.1f;
        [Tooltip("The difference between the last recorded distance and the current distance that is reqired before recording a new position")]
        public float distanceToRecord = 2f;
    }
}
