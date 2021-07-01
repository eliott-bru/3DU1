using UnityEngine;

public class SecondPressurePlateScript : MonoBehaviour
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
            openableDoor.Lock();
        }
    }
}
