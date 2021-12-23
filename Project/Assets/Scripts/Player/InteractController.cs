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
    //private Material objectMaterial;
    private float turnObjectTimer;
    private float turnObjectAngle;
    private string interactButton = "Fire1";
    private bool gridPlacement;

    void Start()
    {
        crosshairObject.sprite = defaultCrosshair;
        handAnimator = hand.GetComponent<Animator>();
        handManager = hand.GetComponent<HandManager>();
    }

    void Update()
    {
        Tools();

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
                            interactButton = "Fire1";
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
                if (hit.collider)
                {
                    if (!hit.collider.GetComponent<AiAgent>())
                    {
                        if (Input.GetButton(interactButton)
                            && !handAnimator.GetCurrentAnimatorStateInfo(0).IsName("HandAction"))
                        {
                            actionTimer = actionDelay;
                            handAnimator.SetTrigger(handManager.handItem.itemData.animation.ToString());

                            if (canInteract)
                            {
                                smart.Interact();
                                StartCoroutine(InteractFeedback(smart.gameObject));

                                if (smart.particle)
                                    Instantiate(smart.particle, hit.point, new Quaternion()).transform.LookAt(transform.position);
                                
                                handManager.handItem.RemoveDurability(handManager);
                            }
                        }
                    }
                    else
                    {
                        if (Input.GetButtonDown(interactButton)
                            && !handAnimator.GetCurrentAnimatorStateInfo(0).IsName("HandAction"))
                        {
                            actionTimer = actionDelay;
                            handAnimator.SetTrigger(handManager.handItem.itemData.animation.ToString());
                        }
                    }
                }
                else
                {
                    if (Input.GetButton(interactButton)
                            && !handAnimator.GetCurrentAnimatorStateInfo(0).IsName("HandAction"))
                    {
                        actionTimer = actionDelay;
                        handAnimator.SetTrigger(handManager.handItem.itemData.animation.ToString());
                    }
                }
            }
        }
    }



    #region Items
    float foodTimer; 
    private FurnitureObject furniture = null;
    private void UseItem()
    {
        GetComponent<FirstPersonController>().isEating = false;
        if (handManager.handItem.itemData)
        {
            switch (handManager.handItem.itemData.itemType)
            {
				#region Furniture
				case ItemData.ItemType.Furniture:
                    turnObjectTimer -= Time.deltaTime;
                    // If exists a placing object, destroy it
                    if (placingObject)
                    {
                        if (placingObject.name != handManager.handItem.itemData.itemName.english)
                            Destroy(placingObject);
                    }

                    // Load the furniture prefab
                    GameObject prefab = Resources.Load<GameObject>("Furnitures/" + handManager.handItem.itemData.itemName.english);

                    // Instantiate the furniture, if it isn't instantiated yet
                    if (!placingObject)
                    {
                        placingObject = Instantiate(prefab, GameObject.Find("Furnitures").transform);
                        placingObject.layer = 2;
                        placingObject.name = placingObject.name.Replace("(Clone)", "");
                        placingObject.transform.eulerAngles = new Vector3(0, turnObjectAngle, 0);
                        foreach (Collider col in placingObject.GetComponents<Collider>())
                        {
                            col.isTrigger = true;
                        }

                        furniture = placingObject.GetComponent<FurnitureObject>();
                        furniture.redMaterial = redMaterial;
                        furniture.isPlacing = true;
                    }

                    // Controls
                    if (Input.GetButton("Turn") && turnObjectTimer <= 0)
                    {
                        turnObjectAngle += 45;
                        placingObject.transform.eulerAngles = new Vector3(0, turnObjectAngle, 0);
                        turnObjectTimer = actionDelay / 2;
                    }
                    else if (Input.GetButtonDown("Turn"))
                    {
                        turnObjectAngle += 45;
                        placingObject.transform.eulerAngles = new Vector3(0, turnObjectAngle, 0);
                        turnObjectTimer = actionDelay / 2;
                    }

                    // If placement furniture object exists
                    if (placingObject)
                    {
                        // Snap to grid
                        if (gridPlacement)
                            placingObject.transform.position = new Vector3((int)hitPoint.x, hitPoint.y, (int)hitPoint.z);
                        else
                            placingObject.transform.position = hitPoint + (Vector3.up  * 0.015f);

                        // Placement
                        if (Vector3.Distance(transform.position, placingObject.transform.position) < placingRange)
                        {
                            if (furniture != null)
                            {
                                // Overlapping materials
                                if (!furniture.isOverlapping)
                                    placingObject.GetComponent<Renderer>().material = furniture.originalMaterial;
                                else
                                    placingObject.GetComponent<Renderer>().material = redMaterial;

                                // Place item
                                if (Input.GetButtonDown("Fire1"))
                                {
                                    if (!furniture.isOverlapping)
                                    {
                                        // Enable components
                                        furniture.isPlacing = false;
                                        foreach (Collider col in placingObject.GetComponents<Collider>())
                                        {
                                            col.isTrigger = false;
                                        }
                                        handManager.PlaceItem();
                                        placingObject.layer = 0;
                                        placingObject = null;
                                        furniture.isOverlapping = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // If far from player, red material to it
                            placingObject.GetComponent<Renderer>().material = redMaterial;
                        }
                    }

                    if (hit.collider) hitPoint = hit.point;

                    break;
                    #endregion
                #region Food
				case ItemData.ItemType.Food:
                    if (Input.GetButton("Fire2") && handManager.handItem.amount > 0 && GetComponent<HealthController>().currentHp < GetComponent<HealthController>().maxHp)
                    {
                        foodTimer += Time.deltaTime;
                        handAnimator.SetBool("IsEating", true);
                        GetComponent<FirstPersonController>().isEating = true;

                        if (foodTimer > 1.5f)
                        {
                            foodTimer = 0;
                            handAnimator.SetBool("IsEating", false);
                            GetComponent<FirstPersonController>().isEating = true; 
                            FoodData food = handManager.handItem.itemData as FoodData;
                            food.UseItem(gameObject);
                            handManager.RemoveItem(handManager.handItem);
                        }
                    }
                    else
                    {
                        foodTimer = 0;
                        handAnimator.SetBool("IsEating", false);
                        GetComponent<FirstPersonController>().isEating = false;
                    }

                    if (handAnimator.GetBool("IsEating"))
                        handManager.gameObject.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime / 2;
                    else
                        handManager.transform.localScale = new Vector3(1, 1, 1);

                    break;
				#endregion

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
                        Destroy(placingObject);
                    }
                }
            }
        }
    }

    #endregion

    private void Tools()
	{
        if (Input.GetButtonDown("SnapToGrid"))
		{
            gridPlacement = !gridPlacement;
		}
	}

    private IEnumerator InteractFeedback(GameObject obj)
	{
        Vector3 scale = obj.transform.localScale;

        obj.transform.localScale = scale * 0.98f;
        yield return new WaitForSeconds(0.1f);
        obj.transform.localScale = scale;
	}
}
