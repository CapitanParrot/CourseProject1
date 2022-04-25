using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    public string MethodName;

    private bool isPlayerClosely = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerClosely)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Invoke(MethodName, 0);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            isPlayerClosely = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).tag.Equals("PressE"))
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            isPlayerClosely = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).tag.Equals("PressE"))
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.tag.Equals("Player"))
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            Invoke(MethodName,0);
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    void Baton()
    {
        PlayerManager.Instance.AddMaxHealth(2);
        PlayerManager.Instance.Heal(2);
    }
}
