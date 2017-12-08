using System.Collections.Generic;
using UnityEngine;

namespace LineUp
{
    public static class DisplayMethods 
    {
        public static List<string> sessionId = new List<string>();
        public static List<List<Vector3>> sessionsMovmentData = new List<List<Vector3>>();

        public static void SeperateAndStoreResults(string text)
        {
            char[] separators = { '#' }; //Define all the characters we want to use to seperate the string we recive from the php script
            string[] results = text.Split(separators); //Split the string into an array so we can go through each one

            switch (results[0])
            {
                case "[Sessions]":  //If the first string is the sessions tag then sort the results into the sessions list
                    SetUpSessionIds(results);
                    break;
                case "[MovementData]": //If the first string is the movement data tag then sort the data into the movment data list
                    SetUpMovementDataSessions(results);
                    break;
            }
        }

        public static void SetUpSessionIds(string[] sessions)
        {
            sessionId.Clear(); //Clear any old data from the array

            for (int i = 1; i < sessions.Length; i++) //Start the for loop from 1 so we don't include the first result
            {
                sessionId.Add(sessions[i]);
            }
        }

        public static void SetUpMovementDataSessions(string[] dataCollection)
        {
            List<string> movementData = new List<string>();

            for (int i = 1; i < dataCollection.Length; i++) //Get rid of the tag from the first result
            {
                movementData.Add(dataCollection[i]);
            }

            SeperateAndCovertMovementData(movementData.ToArray());
        }

        public static void SeperateAndCovertMovementData(string[] sessionsData)
        {
            sessionsMovmentData.Clear(); //Clear any old data from the list

            for (int i = 0; i < sessionsData.Length; i++) //Go through each session
            {
                sessionsMovmentData.Add(new List<Vector3>()); //Create a new session in the list

                char[] separators = { '|' };
                string[] results = sessionsData[i].Split(separators); //Split the string up into each Vector3

                for (int y = 0; y < results.Length - 1; y++) //Don't get the last value as it's blank
                {
                    Vector3 point = GetVector3FromJson(results[y]);
                    sessionsMovmentData[i].Add(point); //Add the result to the sessions list
                }
            }
        }

        public static Vector3 GetVector3FromJson(string json)
        {
            return JsonUtility.FromJson<Vector3>(json);
        }
    }
}
