using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderSetter : MonoBehaviour
{
    // ��������� ������, ����� �������� ����� ��� � ���������� ���������.
    void Start()
    {
        gameObject.GetComponent<Slider>().value = AudioManager.Instance.SoundValue;
    }
}
