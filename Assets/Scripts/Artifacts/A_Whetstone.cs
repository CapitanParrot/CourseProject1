using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Whetstone : MonoBehaviour, IArtifact
{
    private bool isPlayerClosely = false;

    public string artifactName;
    public string description;

    public string Name => artifactName;

    public string Description => description;


    // Update is called once per frame
    void Update()
    {
        if (isPlayerClosely)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.SetParent(ArtifactManager.Instance.transform, false);
                ArtifactManager.Instance.PlayArtifact(gameObject);
                gameObject.SetActive(false);
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

    public void Action(Managers args)
    {
        args.PlayerManager.AddBonusDamage(1);
    }

    public void UndoAction(Managers args)
    {
        args.PlayerManager.AddBonusDamage(-1);
    }
}
