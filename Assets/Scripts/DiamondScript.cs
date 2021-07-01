using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondScript : MonoBehaviour, Interactable
{
    public bool CanInterract(Transform actor)
    {
        return true;
    }

    public void Interact(Transform actor)
    {
        PlayerScript player = actor.GetComponent<PlayerScript>();
        if (player != null)
        {
            player.Win();
        }
    }


}
