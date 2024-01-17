using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class WeatherController : MonoBehaviour
{
    public enum WeatherType
    {
        GoodWeather,
        GoodWeatherWithLightWind,
        BadWeatherRainy,
        BadWeatherFoggy,
        BadWeatherRainyAndFoggy
    }


    [Header("Main Settings"), SerializeField]
    private float weatherChangeTime = 60f;
    [Header("Fog Settings"), SerializeField]
    private Volume volume;
    [SerializeField, Range(0, 1f)]
    private float fogIntensity;
    [Header("Rain Settings"), SerializeField]
    private VisualEffect rainVFX;
    [SerializeField, Range(0, 1f)]
    private float rainIntensity;

    private float lastWeatherChangeTime;
    private float fogFadeTime = 10f;
    private float rainFadeTime = 1f;
    public static event Action<WeatherType> OnWeatherChanged;

    private void Start()
    {
        lastWeatherChangeTime = Time.time;
        ChangeWeather();
    }

    private void Update()
    {
        if (Time.time - lastWeatherChangeTime >= weatherChangeTime)
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
                SetGoodWeather();
                break;
            case WeatherType.GoodWeatherWithLightWind:
                SetGoodWeatherWithLightWind();
                break;
            case WeatherType.BadWeatherRainy:
                SetBadWeatherRainy();
                break;
            case WeatherType.BadWeatherFoggy:
                SetBadWeatherFoggy();
                break;
            case WeatherType.BadWeatherRainyAndFoggy:
                SetBadWeatherRainyAndFoggy();
                break;
        }

        NotifyWeatherChanged(newWeather);
    }

    private void SetGoodWeather()
    {
        StartCoroutine(ChangeFog(false));
        StartCoroutine(ChangeRain(false));
    }

    private void SetGoodWeatherWithLightWind()
    {
        StartCoroutine(ChangeFog(false));
        StartCoroutine(ChangeRain(false));
    }

    private void SetBadWeatherRainy()
    {
        StartCoroutine(ChangeFog(false));
        StartCoroutine(ChangeRain(true));
    }

    private void SetBadWeatherFoggy()
    {
        StartCoroutine(ChangeFog(true));
        StartCoroutine(ChangeRain(false));
    }

    private void SetBadWeatherRainyAndFoggy()
    {
        StartCoroutine(ChangeFog(true));
        StartCoroutine(ChangeRain(true));
    }

    private IEnumerator ChangeFog(bool add)
    {
        VolumeProfile profile = volume.sharedProfile;

        if (!profile.TryGet<Fog>(out var fog))
        {
            fog = profile.Add<Fog>(false);
        }

        float startValue = fog.meanFreePath.value;
        float endValue = add ? fogIntensity * 10 : 1000f;

        float elapsedTime = 0f;

        while (elapsedTime < fogFadeTime)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / fogFadeTime);
            elapsedTime += Time.deltaTime;

            fog.meanFreePath.value = Mathf.Lerp(startValue, endValue, t);

            yield return null;
        }
    }

    private IEnumerator ChangeRain(bool add)
    {
        float startValue = rainVFX.GetFloat("Intensity");
        float endValue = add ? rainIntensity : 0f;

        float elapsedTime = 0f;

        while (elapsedTime < rainFadeTime)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / rainFadeTime);
            elapsedTime += Time.deltaTime;

            rainVFX.SetFloat("Intensity", Mathf.Lerp(startValue, endValue, t));

            yield return null;
        }
    }

    private void NotifyWeatherChanged(WeatherType newWeather)
    {
        OnWeatherChanged?.Invoke(newWeather);
    }
}
