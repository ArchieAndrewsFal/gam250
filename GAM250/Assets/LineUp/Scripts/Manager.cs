using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUp
{
    public class Manager : MonoBehaviour
    {
        public string newName = "Test";

        private string createTableURL = "http://185.52.2.95/StartNewData.php";

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(CreateNewTable());
            }
        }


        IEnumerator CreateNewTable()
        {
            string post = createTableURL + "tableName=" + WWW.EscapeURL(newName);
            WWW tablePost = new WWW(post);
            yield return tablePost;

            if (tablePost.error != null)
                Debug.Log("Ding Table Ready");
        }
    }
}
