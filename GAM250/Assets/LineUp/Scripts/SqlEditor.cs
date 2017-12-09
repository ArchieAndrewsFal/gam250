using System.Collections;
using UnityEngine;
using System;

namespace LineUp
{
    [ExecuteInEditMode]
    public class SqlEditor : MonoBehaviour
    {
        public string publicResult = "";


        public void CreateNewData(string startingMovmentData, MovmentData newData)
        {
            StartCoroutine(SendData(startingMovmentData, newData, DateTime.Now));
        }

        IEnumerator SendData(string startingMovmentData, MovmentData newData, DateTime date)
        {
            WWWForm form = new WWWForm(); //Create an empty form to post to the php script
            form.AddField("movementData", startingMovmentData); //Add the starting movement data to the form
            string dateString = date.Year + "-" + date.Month + "-" + date.Day; //Put the date into the format that an SQL reads e.g. YYYY/MM/DD
            form.AddField("date", dateString);

            WWW postRequest = new WWW(LineUpSqlSettings.startNewDataPhp, form); //Post the form to the php script

            yield return postRequest; //Wait for the script to finish downloading 

            //print(postRequest.text); //Print the result of the page which is the auto increment value given to this row in the table
            int resultID = 0;
            int.TryParse(postRequest.text, out resultID); //Convert the string we got from the php page to an int so we can store it as an id
            newData.id = resultID; //Store the id in the copy of the movment data we sent over
        }

        public void UpdateData(int testID, string movementData)
        {
            WWWForm form = new WWWForm(); //Create an empty form to post to the php script
            form.AddField("idToEdit", testID); //Add the stored id we got from the sql database to the form. We use this to grab the same row to edit
            form.AddField("newMovementData", movementData); //Add the new movement data to the form

            new WWW(LineUpSqlSettings.updateDataPhp, form); //Post the form to the php script
        }
    }
}

