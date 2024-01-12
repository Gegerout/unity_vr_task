using System;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public enum WeatherType
    {
        GoodWeather,
        BadWeather
        /*
        Fog,
        Storm,
        ModerateWaves,
        StrongWaves*/
    }

    [Header("Fog Settings"), SerializeField]
    private float fogDensity = 0.2f;
    [SerializeField]
    private float fogChangeSpeed = 0.5f;

    private float lastWeatherChangeTime;

    public static event Action<WeatherType> OnWeatherChanged;

    private void Start()
    {
        lastWeatherChangeTime = Time.time;
        ChangeWeather();
    }

    private void Update()
    {
        if (Time.time - lastWeatherChangeTime >= 5f)
        {
            ChangeWeather();
            lastWeatherChangeTime = Time.time;
        }
    }

    private void ChangeWeather()
    {
        WeatherType newWeather = (WeatherType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeatherType)).Length);

        switch (newWeather)
        {
            case WeatherType.GoodWeather:
                break;
            case WeatherType.BadWeather:
                break;
        }

        NotifyWeatherChanged(newWeather);
    }

    /*private void SetGoodWeather()
    {
        StartCoroutine(ChangeFog(fogDensity, fogChangeSpeed, false));
    }

    private IEnumerator ChangeFog(float targetDensity, float speed, bool add)
    {
        if(add)
        {
            RenderSettings.fog = true;
            while (RenderSettings.fogDensity < targetDensity)
            {
                RenderSettings.fogDensity += speed * Time.deltaTime;
                yield return null;
            }
        } else
        {
            while (RenderSettings.fogDensity > 0f)
            {
                RenderSettings.fogDensity -= speed * Time.deltaTime;
                if(RenderSettings.fogDensity <= 0f)
                {
                    RenderSettings.fog = false;
                }
                yield return null;
            }
        }
    }*/

    private void NotifyWeatherChanged(WeatherType newWeather)
    {
        OnWeatherChanged?.Invoke(newWeather);
    }
}
