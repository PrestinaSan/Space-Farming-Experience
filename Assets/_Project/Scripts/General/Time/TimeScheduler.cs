using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace TS
{
    public class TimeScheduler : MonoBehaviour
    {
        [SerializeField] private List<Schedule> _schedule;

        private void Start()
        {
            TimeSystem.TimeSystemChanged += CheckSchedule;
        }

        private void OnDestroy()
        {
            TimeSystem.TimeSystemChanged -= CheckSchedule;
        }

        private void CheckSchedule(object sender, TimeSpan newTime)
        {
            var schedule = _schedule.FirstOrDefault(s =>
            s.hour == newTime.Hours &&
            s.minute == newTime.Minutes
            );

            schedule?.action?.Invoke();
        }

        [Serializable] private class Schedule
        {
            public int hour;
            public int minute;
            public UnityEvent action;
        }
    }
}

