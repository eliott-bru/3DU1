using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikesFloorScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponentInChildren<Damageable>();
        if (damageable != null)
        {
            damageable.Damage(10000);
        }
    }
}
