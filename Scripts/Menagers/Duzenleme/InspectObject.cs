using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectObject : MonoBehaviour
{

    private PickUpObject pickUpObjectScript;
    private InputMenager inputMenagerScript;
    
    public GameObject inspectingPanel;
    public GameObject inspectingObject;


    [SerializeField] private Transform inspectPosition;
    private Vector3 originalObejctPosition;
    private Vector3 lastMousePosition;

    public float rotationSpeed = 100f;

    private bool isDragging = false;
    public bool isInspecting = false;

    RaycastHit hit;
    

    void Start()
    {
        pickUpObjectScript = FindObjectOfType<PickUpObject>();
        inputMenagerScript = FindObjectOfType<InputMenager>();

        string scriptName = this.GetType().Name;
        Debug.Log("Hello There is " + scriptName);
        Debug.Log("Inspect Control " + isInspecting);
    }


    void Update()
    {
        if (!pickUpObjectScript.isHolding && !isInspecting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.TryGetComponent(out ICollectable collectObject))
                    {
                        collectObject.CollectableObjects();
                        inspectingObject = hit.collider.gameObject;
                        InspectItem();

                    }
                }
            }
        }
        else if (pickUpObjectScript.isHolding && !isInspecting) //Elimde obje varsa
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                inspectingObject = pickUpObjectScript.holdingObject;
                InspectItem();
            }
        }
        else if (isInspecting&&!pickUpObjectScript.isHolding)
        {
            InspectingItem();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                BackObjectTransform();
            }
        }
    }

    private void InspectItem()
    {
        inputMenagerScript.enabled = false;
        originalObejctPosition = inspectingObject.transform.position;
        Debug.Log("Inceliyon");
        
        inspectingPanel.SetActive(true);
        
        var rigidbody = inspectingObject.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        inspectingObject.transform.position=inspectPosition.position;
        
        isInspecting = true;
        pickUpObjectScript.isHolding = false;
    }

    private void InspectingItem()
    {
   
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 5f);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);
      
        inspectingObject.transform.position =
            Vector3.Lerp(inspectingObject.transform.position, worldPosition, 0.2f);
        

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && inspectingObject != null)
        {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;

            float rotationX = deltaMousePosition.y * rotationSpeed * Time.deltaTime;
            float rotationY = -deltaMousePosition.x * rotationSpeed * Time.deltaTime;

            inspectingObject.transform.Rotate(Camera.main.transform.up, rotationY, Space.World);
            inspectingObject.transform.Rotate(Camera.main.transform.right, rotationX, Space.World);

            lastMousePosition = Input.mousePosition;
        }
        

       
    }
    private void BackObjectTransform()
    {
        inspectingObject.transform.position = Vector3.Lerp(inspectingObject.transform.position,originalObejctPosition,0.2f);
        if (originalObejctPosition == pickUpObjectScript.handPosition.position)
        {
            pickUpObjectScript.isHolding = true;
        }
        inspectingPanel.SetActive(false);
        inspectingObject = null;
        isInspecting = false;
        inputMenagerScript.enabled = true;
    }
}
