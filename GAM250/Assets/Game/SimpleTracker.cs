using UnityEngine;
using LineUp;

public class SimpleTracker : MonoBehaviour
{
    public string trackerTag = "";

    private void OnEnable()
    {
        Manager.tracker.StartTracker(transform, trackerTag);
    }

    private void OnDestroy()
    {
        Manager.tracker.EndTracker(transform);
    }
}
