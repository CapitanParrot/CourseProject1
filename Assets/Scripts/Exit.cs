using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    //public GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //GameManager GM = GameManager.GetComponent<GameManager>();
            //GM.NextLevel();
            GameManager.Instance.NextLevel();
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
