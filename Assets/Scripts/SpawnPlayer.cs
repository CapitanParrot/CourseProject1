using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    // ��������� ������ ��� ����� ����� ������.
    void Start()
    {
        GameManager.Instance.PlayerInstance.transform.position = transform.position;
    }
}
