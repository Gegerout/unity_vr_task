using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherTest : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        WeatherController.OnWeatherChanged += HandleWeatherChanged;
    }

    private void OnDisable()
    {
        WeatherController.OnWeatherChanged -= HandleWeatherChanged;
    }

    private void HandleWeatherChanged(WeatherController.WeatherType newWeather)
    {
        Debug.Log("Weather changed to: " + newWeather);
    }
}
