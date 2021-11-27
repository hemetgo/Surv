using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    [Range(1, 10)] private float interactRange;
    
    [SerializeField]
    private Transform head;
    [SerializeField]
    private GameObject hand;
    [SerializeField]
    [Range(0, 1)] private float actionDelay;

    [Header("Furniture Placement")]
    [SerializeField]
    [Range(1, 20)] private float placingRange;
    [SerializeField]
    private Material redMaterial;


    [Header("GUI")]
    [SerializeField]
    private Image crosshairObject;
    [SerializeField]
    private Sprite defaultCrosshair;

    //Aux
    private Animator handAnimator;
    private float actionTimer;
    private SmartObject smart;
    private bool canInteract;
    private RaycastHit hit;
    private Vector3 hitPoint;
    private GameObject placingObject;
    private float currentRange;
    private HandManager handManager;
    private Material objectMaterial;
    private float turnObjectTimer;
    private float turnObjectAngle;
    private string interactButton = "Fire1";

    void Start()
    {
        crosshairObject.sprite = defaultCrosshair;
        handAnimator = hand.GetComponent<Animator>();
        handManager = hand.GetComponent<HandManager>();
    }

    void Update()
    {
        InteractControl();
        UseItem();
    }



    private void InteractControl()
    {
        if (Time.timeScale != 0)
        {
            actionTimer -= Time.deltaTime;

            if (handManager.handItemData.itemType == Item.ItemType.Furniture) currentRange = 9999;
            else currentRange = interactRange;

            if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, currentRange))
            {
                if (hit.collider.GetComponent<SmartObject>())
                {
                    smart = hit.collider.GetComponent<SmartObject>();
                    if (smart.CanInteract(hand)) canInteract = true;
                    else canInteract = false;

                    switch (smart.GetObjectType())
                    {
                        case SmartObject.ObjectType.Furniture:
                            interactButton = "Fire2";
                            currentRange = interactRange * 1.5f;

                            if (Input.GetButtonDown("Lock")) smart.GetComponent<FurnitureObject>().ToggleKinematic();
                            break;
                        default:
                            interactButton = "Fire1";
                            currentRange = interactRange;
                            break;
                    }

                    if (!smart || smart == null) interactButton = "Fire1";
                }
                else
                {
                    canInteract = false;
                    smart = null;
                }
            }
            else
            {
                canInteract = false;
                smart = null;
            }

            if (actionTimer <= 0)
            {
                if (Input.GetButton(interactButton))
                {
                    actionTimer = actionDelay;
                    handAnimator.SetTrigger("Action");

                    if (canInteract)
                    {
                        smart.Interact();

                        if (smart.particle)
                            Instantiate(smart.particle, hit.point, new Quaternion()).transform.LookAt(transform.position);
                    }
                }
            }
        }

    }



    #region Items
    private void UseItem()
    {
        switch (handManager.handItemData.itemType)
        {
            case Item.ItemType.Furniture:
                turnObjectTimer -= Time.deltaTime;
                if (placingObject)
                {
                    if (placingObject.name != handManager.handItemData.itemName)
                        Destroy(placingObject);
                }

                if (!placingObject)
                {
                    placingObject = Instantiate(handManager.handItemData.GetPrefab(), GameObject.Find("Furnitures").transform);
                    placingObject.name = placingObject.name.Replace("(Clone)", "");
                    placingObject.transform.eulerAngles = new Vector3(0, turnObjectAngle, 0);
                }
                if (!objectMaterial && objectMaterial == null) 
                    objectMaterial = placingObject.GetComponent<Renderer>().material;

                // Disable components
                placingObject.GetComponent<Rigidbody>().isKinematic = true;
                placingObject.GetComponent<FurnitureObject>().enabled = false;
                foreach (Collider col in placingObject.GetComponents<Collider>())
                {
                    col.enabled = false;
                }

                // Controls
                if (Input.GetButton("Turn") && turnObjectTimer <= 0)
                {
                    turnObjectAngle += 45;
                    placingObject.transform.eulerAngles = new Vector3(0, turnObjectAngle, 0);
                    turnObjectTimer = actionDelay / 2;
                } else if (Input.GetButtonDown("Turn"))
                {
                    turnObjectAngle += 45;
                    placingObject.transform.eulerAngles = new Vector3(0, turnObjectAngle, 0);
                    turnObjectTimer = actionDelay / 2;
                }

                // Snap to grid

                if (placingObject)
                {
                    if (Input.GetButton("Crouch"))
                    {
                        
                        placingObject.transform.position = new Vector3((int)hitPoint.x, hitPoint.y, (int)hitPoint.z);
                    }
                    else
                    {
                        placingObject.transform.position = hitPoint;
                    }


                    if (Vector3.Distance(transform.position, placingObject.transform.position) < placingRange)
                    {
                        placingObject.GetComponent<Renderer>().material = objectMaterial;

                        if (Input.GetButtonDown("Fire1"))
                        {
                            // Enable components
                            placingObject.GetComponent<Rigidbody>().isKinematic = false;
                            placingObject.GetComponent<FurnitureObject>().enabled = true;
                            foreach (Collider col in placingObject.GetComponents<Collider>())
                            {
                                col.enabled = true;
                            }
                            handManager.PlaceItem();
                            placingObject = null;
                            objectMaterial = null;
                        }
                    }
                    else
                    {
                        placingObject.GetComponent<Renderer>().material = redMaterial;
                    }
                }

                if (hit.collider) hitPoint = hit.point;

                break;
            default:
                interactButton = "Fire1";
                break;
        }

        if (handManager.handItemData.itemType != Item.ItemType.Furniture) 
        { 
            if (placingObject) 
            {
                objectMaterial = null; 
                Destroy(placingObject); 
            } 
        }
        
    }
    #endregion
}