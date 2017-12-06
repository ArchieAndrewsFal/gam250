using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUp
{
    public class Manager : MonoBehaviour
    {
        public int testID = 0;
        public string movementData = "";

        private string startNewDataPhp = "185.52.2.95/StartNewData.php";
        private string updateDataPhp = "185.52.2.95/UpdateData.php";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(SendData());

                if(Input.GetKeyDown(KeyCode.E))
                UpdateData();
        }

        IEnumerator SendData()
        {
            WWWForm form = new WWWForm();
            form.AddField("movementData", movementData);

            WWW postRequest = new WWW(startNewDataPhp, form);
            yield return postRequest;
            Debug.Log(postRequest.text);
        }

        void UpdateData()
        {
            WWWForm form = new WWWForm();
            form.AddField("idToEdit", testID);
            form.AddField("newMovementData", movementData);

            WWW postRequest = new WWW(updateDataPhp, form);
        }
    }
}
