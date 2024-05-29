using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Transform handPosition;//Elin olması gereken pozisyon

    private InspectObject inspectObjectScript;
    
    private Rigidbody holdingObjectRigidbody;//Eldeki objenin Rigidbodysi
    
    public GameObject holdingObject;//Eldeki obje

    public bool isHolding = false;//Elimde obje var mı

    RaycastHit hit;
    
    void Start()
    {
        inspectObjectScript = FindObjectOfType<InspectObject>();
        string scriptName = this.GetType().Name;
        Debug.Log("Hello There is " + scriptName);
        Debug.Log("Holding Control "+ isHolding);
    }

    
    void Update()
    {
        if (!isHolding&&!inspectObjectScript.isInspecting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.TryGetComponent(out IInteractable interactObject))
                    { 
                        interactObject.InteractableObjects();
                       holdingObject = hit.collider.gameObject;
                       PickUpItem();
                       
                    }
                }
            }
        }

        else if (isHolding&&!inspectObjectScript.isInspecting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DropItem();
                
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (isHolding&&!inspectObjectScript.isInspecting)
        {
            holdingObject.transform.position = handPosition.transform.position;
            var rigidBody = holdingObject.GetComponent<Rigidbody>();
            var moveTo=handPosition.transform.position;
            var differance = moveTo - holdingObject.transform.position;
            rigidBody.AddForce(differance*500);
            holdingObject.transform.rotation = handPosition.rotation;
        }
        
    }

    void PickUpItem()
    {
        Debug.Log("Aldin");
        Debug.Log(holdingObject.name);
        isHolding = true;
        holdingObject.transform.position =
            Vector3.Lerp(holdingObject.transform.position, handPosition.transform.position, 0.4f);
        holdingObjectRigidbody = holdingObject.GetComponent<Rigidbody>();
        holdingObjectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;//Rotasyonunu dondur
        holdingObjectRigidbody.drag = 25f;
        holdingObjectRigidbody.useGravity = false;
        

    }

    void DropItem()
    {
        var rigidBody = holdingObject.GetComponent<Rigidbody>();
        rigidBody.drag = 1f;
        rigidBody.useGravity = true;
        rigidBody.constraints = RigidbodyConstraints.None;
        holdingObject = null;
        isHolding = false;
        Debug.Log("biraktin");
    }
    
}
