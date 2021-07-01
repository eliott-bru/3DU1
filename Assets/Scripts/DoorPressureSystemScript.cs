using System;
using System.Collections;
using UnityEngine;

public class DoorPressureSystemScript : MonoBehaviour, Openable
{
    [SerializeField]
    private float openingSpeed;
    [SerializeField]
    private Vector3 openPosition;
    [SerializeField]
    private Vector3 closePosition;
    [SerializeField]
    private float delay;
    [SerializeField]
    private AudioClip firstClip;
    [SerializeField]
    private AudioClip secondClip;
    [SerializeField]
    private AudioClip tickClip;
    [SerializeField]
    private AudioSource tickSource;


    private float maxTime = 10f;
    private float elapsedTime = 0f;
    private bool isOpen = false;
    private bool isClosed = true;
    private bool shouldOpen = false;
    private bool isLocked = false;
    private AudioSource audioSource;
    private Coroutine coroutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldOpen)
        {
            isClosed = false;
            elapsedTime += Time.deltaTime * openingSpeed;
            elapsedTime = Mathf.Clamp(elapsedTime, 0, maxTime);
            transform.localPosition = Vector3.Lerp(closePosition, openPosition, elapsedTime / maxTime);
            if (elapsedTime / maxTime > 0.85f && !isOpen)
            {
                isOpen = true;
                audioSource.Pause();
                audioSource.clip = secondClip;
                audioSource.Play();
            }
        }
        else
        {
            isOpen = false;
            elapsedTime -= Time.deltaTime * openingSpeed;
            elapsedTime = Mathf.Clamp(elapsedTime, 0, maxTime);
            transform.localPosition = Vector3.Lerp(closePosition, openPosition, elapsedTime / maxTime);
            if (elapsedTime / maxTime < 0.15f && !isClosed)
            {
                isClosed = true;
                audioSource.Pause();
                audioSource.clip = secondClip;
                audioSource.Play();
            }
        }
    }

    public void Close()
    {
        if (!isLocked)
        {
            coroutine = StartCoroutine(DelayCoroutine());
        }
    }

    private IEnumerator DelayCoroutine()
    {
        for (int i = 0; i < (int) delay / 0.5f; i++)
        {
            tickSource.PlayOneShot(tickClip);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(delay % 0.5f);
        if (!isLocked)
        {
            audioSource.clip = firstClip;
            audioSource.Play();
            shouldOpen = false;
        }
    }

    public void Open()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        if (!isLocked && !shouldOpen)
        {
            audioSource.clip = firstClip;
            audioSource.Play();
            shouldOpen = true;
        }
       
    }

    public void Lock()
    {
        if (shouldOpen)
        {
            isLocked = true;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }

    public void Open(int index)
    {
        throw new NotImplementedException();
    }

    public void Close(int index)
    {
        throw new NotImplementedException();
    }
}
