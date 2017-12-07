using UnityEngine;
using LineUp;

public class SimpleTracker : MonoBehaviour
{
    private void Start()
    {
        Manager.tracker.StartTracker(transform);
    }

    private void OnDestroy()
    {
        Manager.tracker.EndTracker(transform);
    }
}
