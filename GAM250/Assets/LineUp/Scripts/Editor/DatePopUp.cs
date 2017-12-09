using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LineUp
{
    public class DatePopUp : EditorWindow
    {
        public static int year, month, day;

        public static string[] months;
        public static string[] days;

        public static Viewer viewerCall;
        public static string callType = "";

        public static void Init(Viewer viewer, string call)
        {
            DatePopUp window = (DatePopUp)EditorWindow.GetWindow(typeof(DatePopUp));
            window.Show();

            year = System.DateTime.Now.Year;
            month = System.DateTime.Now.Month;
            day = System.DateTime.Now.Day - 1;

            SetUpMonths();
            SetUpDays();

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
                days[i] = (i + 1).ToString();
            }
        }

        void OnGUI()
        {
            DrawYearChanger();
            DrawMonthChanger();
            DrawDayChanger();

            if (GUILayout.Button("Set Date"))
            {
                string dayString = (day+1).ToString();
                dayString = (dayString.Length == 1) ? "0" + dayString : dayString;
                string finalDateValue = year.ToString() + "-" + (month).ToString() + "-" + dayString;
                viewerCall.ReciveDate(callType, finalDateValue);

                this.Close();
            }
        }

        void DrawYearChanger()
        {
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("<"))
            {
                year--;
                SetUpMonths();
                SetUpDays();
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
                month = (month == 1) ? 12 : month - 1;
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
            day = GUILayout.SelectionGrid(day, days, 7);
            GUILayout.EndVertical();
        }
    }
}
