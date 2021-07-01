using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTorchScript : MonoBehaviour, Interactable
{
    public bool CanInterract(Transform actor)
    {
        return actor.GetChild(0).gameObject.layer != 3;
    }

    public void Interact(Transform actor)
    {
        if (actor.GetChild(0).gameObject.layer != 3)
        {
            this.transform.gameObject.layer = 3;
            this.transform.parent = actor;
            this.transform.SetSiblingIndex(0);
            this.transform.localPosition = new Vector3(0.5f, 0.3f, 0.5f);
            LayerMask newMask = LayerMask.NameToLayer("HoldableObject");
            foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = newMask;
            }
        }
    }
}
