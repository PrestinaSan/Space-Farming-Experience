using System;
using System.Collections;
using UnityEngine;

namespace TS
{
    public class TimeSystem : MonoBehaviour
    {
        public static event EventHandler<TimeSpan> TimeSystemChanged;
        [Tooltip("In seconds")]
        [SerializeField] private float dayLength;
        [SerializeField] private int startHour = 6;
        [SerializeField] private int startMinute = 0;
        [SerializeField] private Transform player;
        public bool isSleeping = false;
        private TimeSpan currentTime;
        private Coroutine addMinuteCoroutine;
        private Coroutine sleepCoroutine;
        private float MinuteLength => dayLength / TimeConstants.minsInDay;

        private IEnumerator AddMinute()
        {
            currentTime += TimeSpan.FromMinutes(1);
            TimeSystemChanged?.Invoke(this, currentTime);
            yield return new WaitForSeconds(MinuteLength);
            StartCoroutine(AddMinute());
        }
        void Start()
        {
            currentTime = new TimeSpan(startHour, startMinute, 0);
            addMinuteCoroutine = StartCoroutine(AddMinute());
        }
        public void Sleep(Transform bed)
        {
            if (isSleeping) return;
            sleepCoroutine = StartCoroutine(SleepRoutine(bed));
        }
        public void SleepCancel()
        {
            StopCoroutine(sleepCoroutine);
            isSleeping = false;
        }

        private IEnumerator SleepRoutine(Transform bed)
        {
            isSleeping = true;
            player.position = bed.position;

            TimeSpan targetTime = new TimeSpan(startHour, startMinute, 0);
            if (currentTime >= targetTime)
                targetTime = targetTime.Add(TimeSpan.FromHours(24));

            double minutesRemaining = (targetTime - currentTime).TotalMinutes;
            float tickInterval = 5f / (float)minutesRemaining;

            StopCoroutine(addMinuteCoroutine);

            while (currentTime.Hours != startHour || currentTime.Minutes != startMinute)
            {
                currentTime += TimeSpan.FromMinutes(1);
                if (currentTime.TotalHours >= 24)
                    currentTime -= TimeSpan.FromHours(24);
                TimeSystemChanged?.Invoke(this, currentTime);
                yield return new WaitForSeconds(tickInterval);
            }

            StartCoroutine(AddMinute());
            isSleeping = false;
        }
    }
    public static class TimeConstants
    {
        public const int minsInDay = 1440;
    }
}

