using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractController : MonoBehaviour
{
    [Header("Settings")]
    [Range(1, 10)] public float interactRange;
    
    public Transform head;
    public GameObject hand;
    [Range(0, 1)] public float actionDelay;

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
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            actionTimer -= Time.deltaTime;

            if (handManager.handItem.itemData)
            {
                if (handManager.handItem.itemData.itemType == ItemData.ItemType.Furniture) currentRange = 9999;
                else currentRange = interactRange;
            }
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
                            //smart.GetComponent<FurnitureObject>().SetOutlineEnabled(true);
                            //if (Input.GetButtonDown("Lock")) smart.GetComponent<FurnitureObject>().ToggleLocked();
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
                if (Input.GetButtonDown(interactButton))
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
        if (handManager.handItem.itemData)
        switch (handManager.handItem.itemData.itemType)
        {
            case ItemData.ItemType.Furniture:
                turnObjectTimer -= Time.deltaTime;
                if (placingObject) 
                {
                    if (placingObject.name != handManager.handItem.itemData.itemName.english)
                        Destroy(placingObject);
                }

                GameObject prefab = Resources.Load<GameObject>("Furnitures/" + handManager.handItem.itemData.itemName.english);

                if (!placingObject)
                {
                    placingObject = Instantiate(prefab, GameObject.Find("Furnitures").transform);
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

        if (handManager.handItem.itemData)
        {
            if (handManager.handItem.itemData.itemType != ItemData.ItemType.Furniture)
            {
                if (placingObject)
                {
                    objectMaterial = null;
                    Destroy(placingObject);
                }
            }
        }
        
    }
    #endregion
}
