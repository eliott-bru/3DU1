using System;
using UnityEngine;

public class MonkeyDoorScript : MonoBehaviour, Openable
{
    [SerializeField]
    private Renderer leftEye;
    [SerializeField]
    private Renderer rightEye;
    [SerializeField]
    private int nbLight;
    private int nbLightLit = 0;
    [SerializeField]
    private Material offMaterial;
    [SerializeField]
    private Material onMaterial;
    private bool shouldOpen = false;
    private bool isOpen = false;
    private bool isClose = true;
    [SerializeField]
    private float openingSpeed;
    [SerializeField]
    private Vector3 openPosition;
    [SerializeField]
    private Vector3 closePosition;
    private float elapsedTime = 0f;
    private float maxTime = 10f;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip firstClip;
    [SerializeField]
    private AudioClip secondClip;

    private void Start()
    {
        leftEye.material = offMaterial;
        rightEye.material = offMaterial;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (shouldOpen)
        {
            isClose = false;
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
            if (elapsedTime / maxTime < 0.15f && !isClose)
            {
                isClose = true;
                audioSource.Pause();
                audioSource.clip = secondClip;
                audioSource.Play();
            }
        }
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void Close(int index)
    {
        nbLightLit--;
        if (index == 0)
        {
            leftEye.material = offMaterial;
        }
        else if (index == 1)
        {
            rightEye.material = offMaterial;
        }
        if (shouldOpen)
        {
            audioSource.clip = firstClip;
            audioSource.Play();
            shouldOpen = false;
        }
    }

    public void Lock()
    {
        throw new NotImplementedException();
    }

    public void Open()
    {
        throw new NotImplementedException();
    }

    public void Open(int index)
    {        
        nbLightLit++;
        if (index == 0)
        {
            leftEye.material = onMaterial;
        }
        else if (index == 1)
        {
            rightEye.material = onMaterial;
        }
        if (nbLight == nbLightLit && !shouldOpen)
        {
            audioSource.clip = firstClip;
            audioSource.Play();
            shouldOpen = true;
        }
    }
}
