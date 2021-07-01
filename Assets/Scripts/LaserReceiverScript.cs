using UnityEngine;

public class LaserReceiverScript : MonoBehaviour, Reflactable
{
    private bool shouldActivate = false;
    private bool isActivated = false;

    [SerializeField]
    private Material offMaterial;
    [SerializeField]
    private Material onMaterial;
    [SerializeField]
    private Renderer crystalRD;
    private float timeActivated;
    [SerializeField]
    private GameObject door;
    private Openable openableDoor;
    [SerializeField]
    private int index;

    private void Start()
    {
        openableDoor = door.GetComponent<Openable>();
    }

    private void FixedUpdate()
    {
        if (shouldActivate)
        {
            timeActivated += Time.deltaTime;
        }
        else
        {
            timeActivated = 0f;
        }
        if (timeActivated >= 0.05f && !isActivated)
        {
            isActivated = true;
            openableDoor.Open(index);
            GetComponent<AudioSource>().Play();
        }

        crystalRD.material = offMaterial;
        shouldActivate = false;
    }

    public void Reflect(Vector3 point, Vector3 normal, Vector3 direction)
    {
        crystalRD.material = onMaterial;
        shouldActivate = true;
    }

    public bool GetStatus()
    {
        return isActivated;
    }
}
