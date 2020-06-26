using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kilnController : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public Transform thisTransform;
    public GameObject thisOnKiln;
    public IInteractable thisMelting;
    public float burnTime;

    void Start()
    {
        thisTransform = transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(thisTransform.childCount > 0)
        {
            thisOnKiln = thisTransform.GetChild(0).gameObject;
            thisMelting = thisOnKiln.GetComponent<IInteractable>();
            thisMelting?.Interact(); 
        }
    }
    public void Interact()
    {
        burnTime = 10;
    }
}
