using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPressurePlateScript : MonoBehaviour
{
    [SerializeField]
    private Transform door;
    private Openable openableDoor;

    private void Start()
    {
        openableDoor = door.GetComponentInChildren<Openable>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (openableDoor != null)
        {
            openableDoor.Open();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (openableDoor != null)
        {
            openableDoor.Close();
        }
    }
}
