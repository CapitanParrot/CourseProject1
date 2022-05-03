using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Управляет настройками звука.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // СЮда записываются все источники звука.
    public List<AudioSource> Sources = new();

    public string SoundSaveName = "Sound";
    public float SoundValue;
    private float defaultSoundValue = 0.5f;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Awake()
    {
        // Финт ушами, так как кнопки применения настроек привязаны к двум разным объектам AudioManager.
        // Приходится убивать объект с предыдущей сцены и заменять его на новый
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }

        LoadSettings();
        ChangeVolume(SoundValue);
    }

    public void AddSource(AudioSource source)
    {
        source.volume = SoundValue;
        Sources.Add(source);
    }

    // Используется в настройках на кнопке принять.
    public void ChangeVolume(Slider newVolume)
    {
        foreach (var audioSource in Sources)
        {
            if (audioSource != null)
            {
                audioSource.volume = newVolume.value;
            }
            
        }

        SoundValue = newVolume.value;
    }

    // Устанавливает всем источникам звук из настроек.
    public void ChangeVolume(float newVolume)
    {
        foreach (var audioSource in Sources)
        {
            if (audioSource != null)
            {
                audioSource.volume = newVolume;
            }
        }

        SoundValue = newVolume;
    }

    // Сохраняет значение слайдера.
    public void SaveSettings(Slider slider)
    {
        PlayerPrefs.SetFloat(SoundSaveName, slider.value);
    }

    public void LoadSettings()
    {
        SoundValue = PlayerPrefs.GetFloat(SoundSaveName, defaultSoundValue);
    }
}
