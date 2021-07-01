using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorLockScript : MonoBehaviour
{
    [SerializeField]
    private LaserReceiverScript receiver;
    [SerializeField]
    private MirrorScript[] mirrors;
    private bool status;


    // Update is called once per frame
    void Update()
    {
        if (!status && receiver.GetStatus())
        {
            status = true;
        }
        if (status)
        {
            foreach (var mirror in mirrors)
            {
                mirror.Lock();
            }
        }
    }
}
