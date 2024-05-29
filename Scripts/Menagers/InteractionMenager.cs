using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InteractionMenager : MonoBehaviour
{
    //Holding Object
    [SerializeField] private GameObject inspectPanel;
    private GameObject holdingObject;
    public GameObject inspectingObject;
    private GameObject collectedObject;
    
    public InputMenager inputMenagerScript;
    
    public Vector3 newScale = new Vector3(50, 50, 50);
    
    private Material holdingObjectMaterial;
    private Material originalObjectMaterial;
    
    private Renderer holdingObjectRenderer;

    public bool holding=true;
    public bool inspecting=true;
    public bool isCollect;
    
    public Transform handPosition;
    public Transform inspectItemPosition;
    
    public Vector3 originalObjectPosition;
    
    RaycastHit hit;

    private int i = 0;

    public GameObject envanterSlot1;
    public GameObject envanterSlot2;
    private GameObject slot1Object;
    private GameObject slot2Object;
    private void Start()
    {
        holding = false;
        inspecting = false;
        inspectPanel.SetActive(false);
        Debug.Log("Hello There is InteractionMenager"+" Holding Control "+holding+" Inspecting Control "+inspecting);
    }

    
    void Update()
    {
        if (!holding&&!inspecting)//Elim bos ise
        {
            if (Input.GetMouseButtonDown(0))//Left Click Pick Up
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.TryGetComponent(out IInteractable interactObject))
                    {
                        interactObject.InteractableObjects();
                        PickUpItem();
                        
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))//Inspect
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.TryGetComponent(out ICollectable collectObject))
                    {
                        collectObject.CollectableObjects();
                        //inspectingObject = hit.collider.gameObject;
                        originalObjectPosition=inspectingObject.transform.position;
                        InspectItem();
                        
                    }
                }
            }
            
            if (Input.GetMouseButtonDown(1))//Right Click Pick Up
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.TryGetComponent(out ICollectable collectObject))
                    {
                        isCollect = true;
                        collectObject.CollectableObjects();
                        holdingObject = hit.collider.gameObject;
                        if (i == 0)
                        {
                            CollectItem1();

                        }
                        else if (i == 1)
                        {
                            CollectItem2();
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                holdingObject = slot1Object;
                holdingObject.transform.position = handPosition.transform.position;
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                holdingObject = slot2Object;
                holdingObject.transform.position = handPosition.transform.position;
            }
            
        }
              
        else if (holding&&!inspecting)
        {
            
            holdingObjectRenderer = holdingObject.GetComponent<Renderer>();
            
           if (Input.GetMouseButtonDown(0))//Drop
           {
               DropItem();
           }
           
           else if (Input.GetKeyDown(KeyCode.Space))//Inspect
           {
               inspectingObject = holdingObject;
               originalObjectPosition=inspectingObject.transform.position;
               InspectItem();
           }
           if (Input.GetMouseButtonDown(1))//Right Click Pick Up
           {
               
               if (i == 0)
               {
                   CollectItem1();

               }
               else if (i == 1)
               {
                   CollectItem2();
               }
            
           }
        }
                
        else if (inspecting)
        {
            inspectingObject.transform.position =
                Vector3.Lerp(inspectingObject.transform.position, inspectItemPosition.position, 0.2f);
            
            inspectItemPosition.Rotate(new Vector3(Input.GetAxis("Mouse Y"),-Input.GetAxis("Mouse X"),0)*Time.deltaTime*900f);
             inputMenagerScript.enabled = false;
             //inspectingObject.GetComponent<Behaviour>().enabled = false;
             if (Input.GetKeyDown(KeyCode.Space))
             {
                 BackObjectTransform();
             }
        }     
          
          
    }

    private void FixedUpdate()
    
    {
        if (holdingObject != null&&holding)
        {
            var rigidBody = holdingObject.GetComponent<Rigidbody>();
            var moveTo=handPosition.transform.position;
            var differance = moveTo - holdingObject.transform.position;
            rigidBody.AddForce(differance*500);
            holdingObject.transform.rotation = handPosition.rotation;
            
        }
        
    }

    void PickUpItem()
    {
        holding = true;
        holdingObject = hit.collider.gameObject;
        holdingObject.transform.position = Vector3.Lerp(holdingObject.transform.position,handPosition.transform.position,0.4f);
        originalObjectMaterial = holdingObject.GetComponent<Material>();
        
        Debug.Log(holdingObject.name);
        var rigidbody = holdingObject.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.drag = 25f;
        rigidbody.useGravity = false;
        
    }
    void DropItem()
    {
        var rigidBody = holdingObject.GetComponent<Rigidbody>();
        holdingObjectMaterial = originalObjectMaterial;
        rigidBody.drag = 1f;
        rigidBody.useGravity = true;
        rigidBody.constraints = RigidbodyConstraints.None;
        holdingObject = null;
        holding = false;
        inspecting = false;
        Debug.Log("biraktin");
    }

    void InspectItem()
    {
        Debug.Log("Inceliyon");
        inspectPanel.SetActive(true);
        
        var rigidbody = inspectingObject.GetComponent<Rigidbody>();
        //rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.drag = 25f;
        rigidbody.useGravity = false;
         inspectingObject.transform.SetParent(inspectItemPosition);


        //holdingObject = null;
        holding = false;
        inspecting = true;
        
    }

    void BackObjectTransform()
    {
        Debug.Log("Hi");
        inspectingObject.transform.SetParent(null);
        if (originalObjectPosition==handPosition.position)
        {
            holding = true;
            holdingObject = inspectingObject;
            
            holdingObject.transform.position = handPosition.position;

        }

        
        else
        {
            inspectingObject.transform.position = originalObjectPosition;
        }
       
        inspectPanel.SetActive(false);
        inputMenagerScript.enabled = true;
        inspecting = false;
    }

    void CollectItem1()
    {
        i ++;
        Debug.Log("envantere attin");
        holdingObject.transform.position = envanterSlot1.transform.position;
        var rigidbody = holdingObject.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        ChangeScale();
        slot1Object = holdingObject;
        //Destroy(holdingObject);
        collectedObject = holdingObject;
        holdingObject = null;
        holding = false;
        isCollect = true;
    }
    void CollectItem2()
    {
        i++;
        Debug.Log("envantere attin");
        holdingObject.transform.position = envanterSlot2.transform.position;
        var rigidbody = holdingObject.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        ChangeScale();
        slot2Object = holdingObject;
        //Destroy(holdingObject);
        collectedObject = holdingObject;
        holdingObject = null;
        holding = false;
        isCollect = true;
    }
    public void ChangeScale()
    {
        if (holdingObject == null)
        {
            Debug.LogError("Target Object is not assigned.");
            return;
        }

        // targetObject'in ölçek değerlerini değiştir
        holdingObject.transform.localScale = newScale;
    }
}
