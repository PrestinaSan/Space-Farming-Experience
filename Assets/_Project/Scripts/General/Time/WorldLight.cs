using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TS
{
    [RequireComponent(typeof(Light2D))]
    public class WorldLight : MonoBehaviour
    {
        private Light2D light2d;
        [SerializeField] private Gradient gradient;

        private void Awake()
        {
            light2d = GetComponent<Light2D>();
            TimeSystem.TimeSystemChanged += OnTimeSystemChanged;
        }

        private void OnDestroy()
        {
            TimeSystem.TimeSystemChanged -= OnTimeSystemChanged;
        }

        private void OnTimeSystemChanged(object sender, TimeSpan newTime)
        {
            light2d.color = gradient.Evaluate(PercentOfDay(newTime));
        }

        private float PercentOfDay(TimeSpan timeSpan)
        {
            return (float)timeSpan.TotalMinutes % TimeConstants.minsInDay / TimeConstants.minsInDay;
        }
    }

}
