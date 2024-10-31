using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDPT_PortalCollision : MonoBehaviour
{
    public GameObject OnOffObject;

    private void OnCollisionEnter(Collision other) 
    {
        OnOffObject.SetActive(true);
    }
}
