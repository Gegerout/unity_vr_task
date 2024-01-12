using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleController : MonoBehaviour
{
    public static event Action<bool> OnDayNightChanged;

    [Header("Day/Night Settings"), SerializeField]
    private float dayDuration = 10.0f;
    [SerializeField]
    private Light sun;
    [SerializeField]
    private Color dayColor = Color.white;
    [SerializeField]
    private Color nightColor = new Color(0.15f, 0.15f, 0.15f);

    private bool isDay = true;
    private float currentCycleTime = 0.0f;

    private void Start()
    {
        SetDayColor();
    }

    private void Update()
    {
        /*
        currentCycleTime += Time.deltaTime;

        if (currentCycleTime > dayDuration)
        {
            ToggleDayNight();
            currentCycleTime = 0.0f;
        }

        float rotationSpeed = 360.0f / dayDuration;
        float rotationAmount = rotationSpeed * Time.deltaTime;
        sun.transform.Rotate(Vector3.right, rotationAmount);

        float t = currentCycleTime / dayDuration;
        RenderSettings.ambientLight = Color.Lerp(nightColor, dayColor, t);*/
        currentCycleTime += Time.deltaTime;

        if (currentCycleTime > dayDuration)
        {
            ToggleDayNight();
            currentCycleTime = 0.0f;
        }

        // Smooth change of ambient light color and intensity
        float t = currentCycleTime / dayDuration;
        RenderSettings.ambientLight = Color.Lerp(nightColor, dayColor, t);
        sun.intensity = Mathf.Lerp(0.0f, 1.0f, t);
    }

    private void ToggleDayNight()
    {
        isDay = !isDay;
        SetDayColor();
        OnDayNightChanged?.Invoke(isDay);
    }

    private void SetDayColor()
    {
        RenderSettings.ambientLight = isDay ? dayColor : nightColor;
    }
}
