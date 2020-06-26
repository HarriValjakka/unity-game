using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour, ILiftable, IInteractable
{
    public Transform thisTransform;
    public bool lifted;

    public float thisMeltingProcess;
    public float thisAnvilProcess;
    public bool isMelting;
    public bool melted;
    
    private void Awake()
    {
        thisTransform = transform;
        thisAnvilProcess = 0;
        thisMeltingProcess = 0;
        isMelting = false;
        melted = false;
    }
    private void Update()
    {

    }
    public void Lift()
    {
        if (lifted == false)
            {
                lifted = true;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().isKinematic = true;
                thisTransform.SetParent(GameObject.Find("Hands").transform);// = GameObject.Find("Hands").transform;
                thisTransform.localPosition = Vector3.zero;
                thisTransform.localEulerAngles = Vector3.zero;
                thisTransform.GetComponent<BoxCollider>().enabled = false;
            }
    }
    public void Interact()
    {
        if (thisTransform.parent == null)
        {
            return;
        }
        if (thisTransform.parent.GetComponent<kilnController>() != null && isMelting == false && melted==false)
        {
            isMelting = true;
            StartCoroutine(Melting());
        }
    }
    IEnumerator Melting()
    {
        while (isMelting == true)
        {
            if (thisMeltingProcess == 10)
            {
                melted = true;
                isMelting = false;
            }
            else if (transform.parent.GetComponent<kilnController>() == null)
            {
                isMelting = false;
            }else
            {
                thisMeltingProcess++;
                Debug.Log("OnCoroutine: " + (int)Time.time);
                yield return new WaitForSeconds(1f);
            }

        }
    }
    public void Smithing()
    {

    }
}