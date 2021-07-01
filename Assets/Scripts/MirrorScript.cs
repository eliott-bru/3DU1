using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorScript : MonoBehaviour, Reflactable, Interactable
{
    [SerializeField]
    LineRenderer lr;
    private float angle = 0f;
    private float targetAngle = 0f;
    private bool isLocked = false;

    private void Start()
    {
        angle = transform.rotation.eulerAngles.y;
        targetAngle = transform.rotation.eulerAngles.y;
    }

    private void FixedUpdate()
    {

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
    }

    private void Update()
    {
        if (angle != targetAngle)
        {
            angle += 90 * Time.deltaTime;
            angle = Mathf.Min(angle, targetAngle);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
        }
    }

    public void Reflect(Vector3 point, Vector3 normal, Vector3 direction)
    {
        if (Vector3.Angle(normal, transform.right) > 30)
        {
            return;
        }
        lr.SetPosition(0, point);
        Vector3 newDirection = Vector3.Reflect(direction, normal);
        RaycastHit hit;
        if (Physics.Raycast(point, newDirection, out hit))
        {
            
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
                if (hit.collider.transform.parent != null)
                {
                    Reflactable reflactable = hit.collider.transform.parent.GetComponent<Reflactable>();
                    if (reflactable != null)
                    {
                        reflactable.Reflect(hit.point, hit.normal, newDirection);
                    }
                }
            }
        }
        else
        {
            lr.SetPosition(1, point + newDirection * 500);
        }
    }

    public void Interact(Transform actor)
    {
        if (!isLocked && angle == targetAngle)
        {
            targetAngle += 90;
            GetComponent<AudioSource>().Play();
        }       
    }

    public bool CanInterract(Transform actor)
    {
        return !isLocked && angle == targetAngle;
    }

    public void Lock()
    {
        isLocked = true;
    }
}
