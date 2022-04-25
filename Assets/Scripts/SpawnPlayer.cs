using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    //public GameObject Player;

    //private GameObject gameManager;

    //private GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        //GM.PlayerInstance.transform.position = transform.position;
        GameManager.Instance.PlayerInstance.transform.position = transform.position;
    }

    void Awake()
    {
        //Instantiate(Player, transform.position, Quaternion.identity);
        
        //gameManager = GameObject.FindWithTag("GameManager");
        //GM = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
