using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    private bool hasBegun = false;

    private void OnCollisionEnter(Collision collision)
    {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.Damage(1000);
        }
        else if (collision.gameObject.layer == 6)
        {
            if (!hasBegun)
            {
                GetComponent<AudioSource>().Play();
                hasBegun = true;
            }
        }
    }
}
