using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��������� ����������� �����.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // ���� ������������ ��� ��������� �����.
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
        // ���� �����, ��� ��� ������ ���������� �������� ��������� � ���� ������ �������� AudioManager.
        // ���������� ������� ������ � ���������� ����� � �������� ��� �� �����
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

    // ������������ � ���������� �� ������ �������.
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

    // ������������� ���� ���������� ���� �� ��������.
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

    // ��������� �������� ��������.
    public void SaveSettings(Slider slider)
    {
        PlayerPrefs.SetFloat(SoundSaveName, slider.value);
    }

    public void LoadSettings()
    {
        SoundValue = PlayerPrefs.GetFloat(SoundSaveName, defaultSoundValue);
    }
}
