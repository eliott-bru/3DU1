using System;
using UnityEngine;

public class DoorFireSystemScript : MonoBehaviour, Openable
{
    private bool shouldOpen = false;
    private bool isOpen = false;
    private float elapsedTime = 0f;
    [SerializeField]
    private float openingSpeed;
    [SerializeField]
    private Vector3 openPosition;
    [SerializeField]
    private Vector3 closePosition;
    private float maxTime = 10f;
    [SerializeField]
    private int nbTorch;
    private int nbTorchLit = 0;
    [SerializeField]
    private AudioClip firstClip;
    [SerializeField]
    private AudioClip secondClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!shouldOpen) 
        {
            if (nbTorch == nbTorchLit)
            {
                shouldOpen = true;

                audioSource.clip = firstClip;
                audioSource.Play();
            }
        }
        else
        {
            elapsedTime += Time.deltaTime * openingSpeed;
            elapsedTime = Mathf.Clamp(elapsedTime, 0, maxTime);
            transform.localPosition = Vector3.Lerp(closePosition, openPosition, elapsedTime / maxTime);
            if (elapsedTime / maxTime > 0.85f && !isOpen)
            {
                isOpen = true;
                audioSource.Stop();
                audioSource.clip = secondClip;
                audioSource.Play();
            }
        }
    }

    public void Open()
    {
        nbTorchLit++;
    }

    public void Open(int index)
    {
        Open();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void Lock()
    {
        throw new NotImplementedException();
    }

    public void Close(int index)
    {
        throw new NotImplementedException();
    }
}
