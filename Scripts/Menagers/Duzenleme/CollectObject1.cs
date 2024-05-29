using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectObject1 : MonoBehaviour
{
    private PickUpObject pickUpObjectScript;
    private InspectObject inspectObjectScript;
    private Inventory inventoryScript;
    
    public GameObject collectingItem;

    public bool collecting;
    
    RaycastHit hit;
    
    void Start()
    {
        pickUpObjectScript= FindObjectOfType<PickUpObject>();
        inspectObjectScript= FindObjectOfType<InspectObject>();
        inventoryScript= FindObjectOfType<Inventory>();
        
        string scriptName = this.GetType().Name;
        Debug.Log("Hello There is " + scriptName);
        //
        
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(1)&&inventoryScript.inventorySlotsNumber!=0)
        {
            if (!inspectObjectScript.isInspecting&&!pickUpObjectScript.isHolding)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.TryGetComponent(out ICollectable collectObject))
                    {
                        collectObject.CollectableObjects();
                        collectingItem = hit.collider.gameObject;
                        CollectItem();
                    }
                }
            }
            else if (inspectObjectScript.isInspecting && !pickUpObjectScript.isHolding)
            {
                collectingItem = inspectObjectScript.inspectingObject;
                inspectObjectScript.isInspecting = false;
                CollectItem();
            }
        }
        else if(Input.GetMouseButtonDown(1)&&inventoryScript.inventorySlotsNumber==0)
        {
            Debug.Log("Envanter Dolu");
        }
        
    }

    void CollectItem()
    {
        if (inventoryScript.inventorySlotsNumber == 0)
        {
            Debug.Log("This is cant be 0");
        }
        inventoryScript.ShowSpriteCollectedItemOnInventory();
        Debug.Log("Topladin");
        //collectingItem = null;
        //Destroy(collectingItem);
        
    }
}
