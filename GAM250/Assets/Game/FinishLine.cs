using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LineUp;

public class FinishLine : MonoBehaviour
{
    public Transform startPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Manager.tracker.EndTracker(other.transform);
            other.enabled = false;
            other.transform.position = startPoint.position;
            other.enabled = true;
        }
    }
}
