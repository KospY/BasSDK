using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class DatetimeActive : MonoBehaviour
    {
        [System.Serializable]
        public class SimpleDateTime
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("SimpleTime", Width = 75)]
            [LabelWidth(40)]
#endif
            public int month;
#if ODIN_INSPECTOR
            [HorizontalGroup("SimpleTime", Width = 60)]
            [LabelWidth(25)]
#endif
            public int day;
#if ODIN_INSPECTOR
            private string hourMinuteString
            {
                get
                {
                    var time = GetHourMinute();
                    return $"Time: {time.hour}:{(time.minute == 0 ? "00" : time.minute)}";
                }
            }

            [LabelText("$hourMinuteString")]
            [LabelWidth(75)]
            [HorizontalGroup("SimpleTime")]
#endif
            [Range(0, 48)]
            public int time;

            public (int hour, int minute) GetHourMinute()
            {
                return (Mathf.FloorToInt(time / 2), 30 * (time % 2));
            }

            public DateTime ToDateTime(int year)
            {
                var time = GetHourMinute();
                DateTime dt = new DateTime(year, month, day, time.hour, time.minute, 0);
                return dt;
            }

            public SimpleDateTime(int month, int day, int time)
            {
                this.month = month;
                this.day = day;
                this.time = time;
            }
        }

#if ODIN_INSPECTOR
        [InlineProperty]
        [HideLabel]
        [Header("From")]
#endif
        public SimpleDateTime activeStart = new SimpleDateTime(6, 17, 36);
#if ODIN_INSPECTOR
        [InlineProperty]
        [HideLabel]
        [Header("Until")]
#endif
        public SimpleDateTime activeEnd = new SimpleDateTime(10, 28, 36);

#if ODIN_INSPECTOR
        [Button]
#endif
        public bool CheckWithinRange()
        {
            DateTime now = DateTime.Now;
            int currentYear = now.Year;
            DateTime startDate = activeStart.ToDateTime(currentYear);
            DateTime endDate = activeEnd.ToDateTime(currentYear);
            // if the end date is earlier/less than the start date, we know the new year happens between the two dates, so we handle it separately
            if (endDate < startDate)
            {
                return now >= startDate || now <= endDate;
            }
            else
            {
                return now >= startDate && now <= endDate;
            }
        }

        private void Awake()
        {
            if (!CheckWithinRange()) gameObject.SetActive(false);
        }
    }
}
