﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUp
{
    [RequireComponent(typeof(SqlEditor))]
    public class Manager : MonoBehaviour
    {
        public static Manager tracker; //This allows us to get access to this class' public methods from any where using the LineUp namespace
        public bool destroyOnLoad = false; //Allows the user to say if they want this Manager to persist through scenes

        public LineUpSettings settings;

        public List<MovmentData> activeTrackers = new List<MovmentData>(); //This list stores all of the active trackers sending data to the server
        private SqlEditor sql;
        private WaitUntil delay;
        private float distance = 0;
        int frameCounter = 0;

        private void Awake()
        {
            MakeSinglton();
            CheckForSettings();
            SetUpSqlRefrence();

            delay = new WaitUntil(() => frameCounter >= settings.framesBetweenCycles);
            StartCoroutine(Cycle()); //Start the cycle
        }

        void SetUpSqlRefrence()
        {
            sql = GetComponent<SqlEditor>();
        }

        void MakeSinglton()
        {
            //Make sure only one instance of the Manager exisits in the scene
            if (tracker != null)
                Destroy(gameObject);

            tracker = this;

            //If the user wants the manager to persist through scenes the set them to not destory on load 
            if (!destroyOnLoad)
                Object.DontDestroyOnLoad(gameObject);
        }

        //Make sure there are values for the manager to use
        void CheckForSettings()
        {
            if (settings == null)
            {
                Debug.LogWarning("No Settings where found for LineUp Manager. Creating defualt settings to use instead", gameObject);
                settings = ScriptableObject.CreateInstance(typeof(LineUpSettings)) as LineUpSettings;
            }
        }

        public void StartTracker(Transform tracker, string tag = "")
        {
            MovmentData newTracker = new MovmentData(); //Create empty movement data to fill with new information

            newTracker.transformToTrack = tracker; //Set the transform we want to track
            newTracker.lastRecordedPosition = tracker.position;
            newTracker.movementString = JsonPosition(newTracker);

            //Create the new row in the sql for this data. Also pass the movment data so it can set the id the sql created for it.
            sql.CreateNewData(newTracker.movementString, newTracker, tag);

            activeTrackers.Add(newTracker);
        }

        string JsonPosition(MovmentData tracker)
        {
            return JsonUtility.ToJson(tracker.lastRecordedPosition) + "|"; //Add the vertical bar after we create the JSON so we can seperate it later on
        }

        //Removed this as it was creating too much GC
        //string CreateJsonFromMovementData(Vector3[] movement)
        //{
        //    string json = "";

        //    for (int i = 0; i < movement.Length; i++)
        //    {
        //        json += JsonUtility.ToJson(movement[i]) + "|";
        //    }

        //    return json;
        //}

        public void EndTracker(Transform tracker)
        {
            for (int i = 0; i < activeTrackers.Count; i++) //Go through each tracker we have stored
            {
                if (activeTrackers[i].transformToTrack == tracker) //If the tracker transform passed here matches the one in the list then remove it
                    activeTrackers.Remove(activeTrackers[i]); //Remove tracker
            }
        }

        IEnumerator Cycle()
        {
            while (true) //Create our own slow Update
            {
                for (int i = 0; i < activeTrackers.Count; i++) //Run any jobs we need to do on all the trackers each cycle
                {
                    CheckIfTrackerIsNull(activeTrackers[i]);

                    if(activeTrackers[i].transformToTrack != null) //Check if the tracker was remove last frame
                        CheckTrackerForUpdate(activeTrackers[i]);
                }
                yield return delay;
                frameCounter = 0;
            }
        }

        private void Update()
        {
            frameCounter++;
        }

        void CheckTrackerForUpdate(MovmentData tracker)
        {
            distance = Vector3.Distance(tracker.transformToTrack.position, tracker.lastRecordedPosition); //Get the distance between the last recorded point and the active point

            if (distance >= settings.distanceToRecord) //Check if the distance between the last recorded point and the current point is large enough to record a new point
            {
                tracker.lastRecordedPosition = tracker.transformToTrack.position;
                tracker.movementString += JsonPosition(tracker);
                sql.UpdateData(tracker.id, tracker.movementString); //Update the row in the sql
            }
        }

        //Go through all the trakers and check if the transform still exsists
        void CheckIfTrackerIsNull(MovmentData tracker)
        {
            if (tracker.transformToTrack == null)
            {
                activeTrackers.Remove(tracker); //Remove the null tracker from the list. Do this here rather than using the EndTracker method as there is no transform to check againts.
                Debug.LogError("Tracker [" + tracker.id + "] was missing, it has been removed from the tracker list and has stopped sending movment data to the server.", gameObject);
            }         
        }
    }
}
