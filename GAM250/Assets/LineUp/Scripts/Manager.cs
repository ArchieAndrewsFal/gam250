using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUp
{
    public class Manager : MonoBehaviour
    {
        public int testID = 0;
        public string movementData = "";

        private string startNewDataPhp = "185.52.2.95/StartNewData.php?";
        private string updateDataPhp = "185.52.2.95/UpdateData.php?";

        public string url;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SendData();

            if(Input.GetKeyDown(KeyCode.E))
                UpdateData();
        }

        void SendData()
        {
            StartCoroutine(PostData());
        }

        void UpdateData()
        {
            StartCoroutine(PostNewData());
        }

        IEnumerator PostData()
        {
            string postString = startNewDataPhp + "movementData=" + movementData;
            WWW post = new WWW(postString);
            yield return post; // Get a result and wait for it to ensure the data was sent

            //Log any errors
            if (post.error != null) 
                Debug.Log(post.error);
        }

        IEnumerator PostNewData()
        {
            string postString = updateDataPhp + "newMovementData=" + movementData + "&idToEdit=" + testID;

            url = postString;

            WWW post = new WWW(postString);
            yield return post; // Get a result and wait for it to ensure the data was sent

            //Log any errors
            if (post.error != null)
                Debug.Log(post.error);
        }
    }
}
