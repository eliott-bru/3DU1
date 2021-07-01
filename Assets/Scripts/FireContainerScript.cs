using UnityEngine;

public class FireContainerScript : MonoBehaviour, Interactable
{
    [SerializeField]
    private bool isLit;
    [SerializeField]
    private ParticleSystem fire;
    [SerializeField]
    private Light fireLight;
    [SerializeField]
    private GameObject door;
    private Openable openableDoor;
    [SerializeField]
    private int index;

    void Start()
    {
        openableDoor = door.GetComponent<Openable>();
        if (isLit)
        {
            GetComponent<AudioSource>().Play();
            fire.Play();
            fireLight.range = 20;
            openableDoor.Open(index);
        }
        else
        {
            fireLight.range = 0;
        }
    }

    public void Interact(Transform actor)
    {
        if (!isLit)
        {
            FireTorchScript fireTorchScript = actor.GetChild(0).GetComponent<FireTorchScript>();
            if (fireTorchScript != null)
            {
                isLit = true;
                openableDoor.Open(index);
                GetComponent<AudioSource>().Play();
                fire.Play();
                fireLight.range = 20;
            }
        }
    }

    public bool CanInterract(Transform actor)
    {
        if (isLit)
        {
            return false;
        }
        else
        {
            FireTorchScript fireTorchScript = actor.GetChild(0).GetComponent<FireTorchScript>();
            return fireTorchScript != null;
        }
    }
}
