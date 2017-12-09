using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUp
{
    public class LineUpSettings : ScriptableObject
    {
        [Tooltip("The frame count between position and null checks in the manager")]
        public int framesBetweenCycles = 60;
        [Tooltip("The difference between the last recorded distance and the current distance that is reqired before recording a new position")]
        public float distanceToRecord = 2f;
    }
}
