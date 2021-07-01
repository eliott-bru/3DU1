using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootScript : MonoBehaviour, Interactable
{
    private bool isBurned = false;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private ParticleSystem particles;
    [SerializeField]
    private BoxCollider blockingVolume;

    public bool CanInterract(Transform actor)
    {
        if (isBurned)
        {
            return false;
        }
        FireTorchScript fireTorchScript = actor.GetChild(0).GetComponent<FireTorchScript>();
        return fireTorchScript != null;
        
    }

    public void Interact(Transform actor)
    {
        if (!isBurned)
        {
            FireTorchScript fireTorchScript = actor.GetChild(0).GetComponent<FireTorchScript>();
            if (fireTorchScript != null)
            {
                isBurned = true;
                StartCoroutine(BurnCoroutine());
            }
        }
    }

    public IEnumerator BurnCoroutine()
    {
        anim.SetBool("burn", true);
        GetComponent<AudioSource>().Play();
        particles.Play();
        yield return new WaitForSeconds(1f);
        blockingVolume.enabled = false;
        GetComponent<AudioSource>().Stop();
    }
}
