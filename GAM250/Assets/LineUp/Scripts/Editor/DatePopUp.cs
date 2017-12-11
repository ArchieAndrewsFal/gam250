using UnityEngine;
using UnityEditor;

namespace LineUp
{
    public class DatePopUp : EditorWindow
    {
        public static int year, month, day;

        public static string[] months; //Store the month names
        public static string[] days; //Store the days as string

        public static Viewer viewerCall; //The viewer that called this popup
        public static string callType = ""; //The tag that was sent 

        public static void Init(Viewer viewer, string call)
        {
            DatePopUp window = (DatePopUp)EditorWindow.GetWindow(typeof(DatePopUp));
            window.Show();

            //Set the active date as todays date
            year = System.DateTime.Now.Year;
            month = System.DateTime.Now.Month;
            day = System.DateTime.Now.Day - 1;

            SetUpMonths(); //Get all the months that are in the year
            SetUpDays(); //Get all the days that are in the month

            viewerCall = viewer;
            callType = call;

            window.ShowPopup();
        }

        public static void SetUpMonths()
        {
            //Set up the months for the selector
            months = new string[12];
            System.DateTime monthCounter = new System.DateTime(year, 1, 1);

            for (int i = 0; i < 12; ++i)
            {
                monthCounter = new System.DateTime(year, i + 1, 1);
                months[i] = monthCounter.ToString("MMMM");
            }
        }

        public static void SetUpDays()
        {
            //Set up the months for the selector
            days = new string[System.DateTime.DaysInMonth(year, month)];

            for (int i = 0; i < System.DateTime.DaysInMonth(year, month); ++i)
            {
                days[i] = (i + 1).ToString(); //Offset the i as arrays start at 0 and days in a month start at 1
            }
        }

        void OnGUI()
        {
            DrawYearChanger();
            DrawMonthChanger();
            DrawDayChanger();

            if (GUILayout.Button("Set Date"))
            {
                string dayString = (day+1).ToString(); //Offset the day as arrays start at 0
                dayString = (dayString.Length == 1) ? "0" + dayString : dayString; //Make sure that if the day is not double digits we add a 0 to the begining of it.
                string finalDateValue = year.ToString() + "-" + (month).ToString() + "-" + dayString; //Format the string to generic date formatting so the SQL server can read it
                viewerCall.ReciveDate(callType, finalDateValue);

                this.Close(); //Close the window
            }
        }

        void DrawYearChanger()
        {
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("<"))
            {
                year--; //Change the year 
                SetUpMonths(); //Re setup the months
                SetUpDays(); //Re setup the days
            }

            year = EditorGUILayout.IntField(year);

            if (GUILayout.Button(">"))
            {
                year++;
                SetUpMonths();
                SetUpDays();
            }

            GUILayout.EndHorizontal();
        }

        void DrawMonthChanger()
        {
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("<"))
            {
                month = (month == 1) ? 12 : month - 1; //Decrease the selected month by one unless it's at the begining then just loop it
                SetUpDays();
            }

            GUILayout.Label(months[month - 1].ToString());

            if (GUILayout.Button(">"))
            {
                month = (month == 12) ? 1 : month + 1;
                SetUpDays();
            }

            GUILayout.EndHorizontal();
        }

        void DrawDayChanger()
        {
            GUILayout.BeginVertical("box");
            day = GUILayout.SelectionGrid(day, days, 7); //Draw the days as a grid and select the active day
            GUILayout.EndVertical();
        }
    }
}
