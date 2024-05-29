using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Book1 : MonoBehaviour,IInteractable,ICollectable
{
    private PickUpObject pickUpObjectScript;
    private InspectObject inspectObjectScript;
    
    
    private GameObject openBook;
    private GameObject normalBook;
    
    private Material originalMaterial;
    public Material hoverMaterial;
    
    void Start()
    {
        pickUpObjectScript= FindObjectOfType<PickUpObject>();
        inspectObjectScript= FindObjectOfType<InspectObject>();
        openBook = GameObject.FindWithTag("book_open");
        //openBook.SetActive(false);
        originalMaterial = GetComponent<Renderer>().material;
        
    }

    
    private void OnMouseEnter()
    {
        if (!pickUpObjectScript.isHolding && !inspectObjectScript.isInspecting)
        {
            GetComponent<Renderer>().material = hoverMaterial;
        }
        if(pickUpObjectScript.isHolding && inspectObjectScript.isInspecting)
        {
            GetComponent<Renderer>().material = originalMaterial;
        }
        
    }

    private void OnMouseExit()
    {
          GetComponent<Renderer>().material = originalMaterial;  
    }

    void Update()
    {
      
     
    }
    
    
    
    //Interfaces
    public void Interactable()
    {
        
    }
    
    public void InteractableObjects()
    {
        Interactable();
    }

    public void Collectable()
    {
        
    }

    public void CollectableObjects()
    {
        Collectable();
    }
    
    
}
