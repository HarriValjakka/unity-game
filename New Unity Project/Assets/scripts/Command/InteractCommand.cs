using System;
using UnityEngine;


    public class InteractCommand : Command
    {
    private Transform thisTransform;
    private IInteractable thisInteractedWith;
    private Vector3 offSet = new Vector3(0,-1,0);

    private const string LayerName = "Interactable";
    private const string Layer2Name = "Table";

    private void Awake()
        {
            thisTransform = transform;
        }

        public override void Execute()
        {
            Physics.Raycast(thisTransform.position + offSet / 2, 
                thisTransform.forward, out var hit, 10f,
                LayerMask.GetMask(LayerName, Layer2Name));
            Debug.DrawRay(transform.position + offSet/2, transform.forward, Color.green,5);
            Debug.Log($"{gameObject.name} is trying to interact!");
            
            if (hit.collider == null) return;
            
            Debug.Log($"{gameObject.name} is interacted with {hit.collider.name}");

            thisInteractedWith = hit.collider.GetComponent<IInteractable>();
            thisInteractedWith?.Interact();
        }
    }
