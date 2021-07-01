using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTriggerScript : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            rock.isKinematic = false;
        }
    }
}
