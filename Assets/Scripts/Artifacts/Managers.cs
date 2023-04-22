using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers
{
    public PlayerManager PlayerManager { get; set; }

    public GameManager GameManager { get; set; }

    public Managers(PlayerManager playerManager, GameManager gameManager)
    {
        PlayerManager = playerManager;
        GameManager = gameManager;
    }
}
