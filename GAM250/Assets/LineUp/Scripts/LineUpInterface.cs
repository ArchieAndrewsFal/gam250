#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUp
{
    [RequireComponent(typeof(Manager), typeof(SqlEditor))]
    public class LineUpInterface : MonoBehaviour
    {
        public Manager manager;
        public SqlEditor sqlEditor;
        public bool initialized = false;

        public void Init()
        {
            manager = GetComponent<Manager>();
            sqlEditor = GetComponent<SqlEditor>();
            initialized = true;
        }
    }
}
#endif

