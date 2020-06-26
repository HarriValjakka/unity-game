using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

public class PickUpCommand : Command
{
    private Transform thisTransform;
    private ILiftable thisInteractedWith;
    private Vector3 offSet = new Vector3(0, -1, 0);
    public Transform thisHands;
    public Transform thisHandsHave;
    public Transform onTable;
    private Vector3 offSetUp = new Vector3(0, 3, 0);

    private const string LayerName = "Interactable";
    private const string Layer2Name = "Table";

    private void Awake()
    {
        thisTransform = transform;
        thisHands = gameObject.transform.GetChild(1);
    }

    public override void Execute()
    {
        //make raycast to check what is iinfrontt of us
        Physics.Raycast(thisTransform.position + offSet / 2,
        thisTransform.forward, out var hit, 5f,
        LayerMask.GetMask(LayerName, Layer2Name));
        Debug.DrawRay(transform.position + offSet / 2, transform.forward, Color.green, 5);
        Debug.Log($"{gameObject.name} is trying to lift!");

        //if player hands got nothing, check if hit was null and if not,
        //check if object is food, put it in player hand
        //if if hitted table and if table got food, take food.
        if (thisHands.childCount < 1)
        {
            if (hit.collider == null) return;

            Debug.Log($"{gameObject.name} is trying to lift {hit.collider.name}");
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(LayerName))
            {
                thisInteractedWith = hit.collider.GetComponent<ILiftable>();
                thisInteractedWith?.Lift();
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer(Layer2Name))
            {
                if (hit.transform.childCount > 0)
                {
                    onTable = hit.transform.GetChild(0);
                    thisInteractedWith = onTable.GetComponent<ILiftable>();
                    thisInteractedWith?.Lift();
                }
            }
        }//if hit is not null check if hit table
         //if hit table, and table is empty, put on table, if table full, but floor, if hit another food, put floor
        else if (hit.collider != null)
        {
            if (thisHands.childCount > 0 && hit.transform.gameObject.layer == LayerMask.NameToLayer(Layer2Name))
            {
                if (hit.transform.childCount < 1)
                {
                    putTable(hit);
                }
                else
                {
                    putFloor();
                }
            }
            else
            {
                putFloor();
            }
        }//if we got nothiing else to do and got food in hand, throw floor
        else if (thisHands.childCount > 0)
        {
            putFloor();
        }
    }
    //throw floor funktiion
    public void putFloor()
    {
        Debug.Log("1 child, put on floor if not hit table");
        thisHandsHave = thisHands.GetChild(0);
        thisHandsHave.transform.SetParent(null);
        thisHandsHave.GetComponent<BoxCollider>().enabled = true;
        thisHandsHave.GetComponent<Rigidbody>().useGravity = true;
        thisHandsHave.GetComponent<Rigidbody>().isKinematic = false;
        thisHandsHave.GetComponent<Food>().lifted = false;
    }
    //put table funktion
    public void putTable(RaycastHit hit)
    {
        Debug.Log("1 child, put on table");
        thisHandsHave = thisHands.GetChild(0);
        thisHandsHave.transform.SetParent(hit.transform);
        thisHandsHave.transform.position = hit.transform.position;
        thisHandsHave.transform.position += (offSetUp / 4);
        thisHandsHave.GetComponent<Food>().lifted = false;
    }
}
