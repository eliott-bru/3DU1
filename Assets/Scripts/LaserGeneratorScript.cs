using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGeneratorScript : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lr;

    private void Start()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
                if (hit.collider.transform.parent != null)
                {
                    Reflactable reflactable = hit.collider.transform.parent.GetComponent<Reflactable>();
                    if (reflactable != null)
                    {
                        reflactable.Reflect(hit.point, hit.normal, transform.forward);
                    }
                }
            }
        }
        else
        {
            lr.SetPosition(1, transform.position + transform.forward * 500);
        }
    }
}
