using System;
using TMPro;
using UnityEngine;

namespace TS
{
    [RequireComponent(typeof(TMP_Text))]
    public class TimeDisplay : MonoBehaviour
    {
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            TimeSystem.TimeSystemChanged += OnTimeSystemChanged;
        }

        private void OnDestroy()
        {
            TimeSystem.TimeSystemChanged -= OnTimeSystemChanged;
        }

        private void OnTimeSystemChanged(object sender, TimeSpan newTime)
        {
            string ampm;
            if (newTime.Hours < 12) ampm = " AM";
            else ampm = " PM";
            int timeDisplay = newTime.Hours;
            if (newTime.Hours > 12)
            {
                timeDisplay = newTime.Hours - 12;
            }
            text.SetText(timeDisplay.ToString() + ampm);
        }
    }
}

