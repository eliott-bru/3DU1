using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorielScript : MonoBehaviour
{
    private int index = 4;
    private int progression = 0;

    private void OnTriggerExit(Collider other)
    {
        PlayerScript player = other.GetComponent<PlayerScript>();
        if (player != null)
        {
            index++;
            if (index > progression)
            {
                progression = index;
                player.SetToolTip(index);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerScript player = other.GetComponent<PlayerScript>();
        if (player != null)
        {
            index--;
        }
    }
}
