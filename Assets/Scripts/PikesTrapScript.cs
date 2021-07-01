using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikesTrapScript : MonoBehaviour
{
    [SerializeField]
    private Transform pressurePlate;
    [SerializeField]
    private Transform pikes;
    private bool isActivated = false;
    [SerializeField]
    private float activationDelay;
    [SerializeField]
    private float desactivationDelay;
    [SerializeField]
    private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated)
        {
            StartCoroutine(PikeCoroutine());
        }
    }

    private IEnumerator PikeCoroutine()
    {
        isActivated = true;
        pressurePlate.Translate(pressurePlate.up * -0.05f);
        pressurePlate.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(activationDelay);
        pikes.Translate(pikes.up * 0.9f);
        GetComponent<AudioSource>().Play();
        Collider[] colliders = Physics.OverlapBox(pikes.position, new Vector3(0.5f, 0.5f, 0.5f));
        for (int i = 0; i < colliders.Length; i++)
        {
            Damageable damageable;
            if (colliders[i].TryGetComponent<Damageable>(out damageable))
            {
                damageable.Damage(damage);
            }
        }
        yield return new WaitForSeconds(desactivationDelay);
        pikes.Translate(pikes.up * -0.9f);
        pressurePlate.Translate(pressurePlate.up * 0.05f);
        isActivated = false;
    }


}
