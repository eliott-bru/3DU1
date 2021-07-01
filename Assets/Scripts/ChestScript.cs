using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour, Interactable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ParticleSystem particles;
    private bool isOpen = false;
    [SerializeField]
    private AudioClip openingClip;
    [SerializeField]
    private AudioClip closingClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool CanInterract(Transform actor)
    {
        return true;
    }

    void Interactable.Interact(Transform actor)
    {
        isOpen = !isOpen;
        particles.Pause();
        particles.Clear();
        animator.SetBool("isOpen", isOpen);
    }

    public void PlayOpeningChestSound()
    {
        audioSource.clip = openingClip;
        audioSource.Play();
    }
    public void PlayClosingChestSound()
    {
        audioSource.clip = closingClip;
        audioSource.Play();
    }
}
