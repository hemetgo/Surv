using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InteractController : MonoBehaviour
{
    [Header("Settings")]
    [Range(1, 10)] public float interactRange;
    
    public Transform head;
    public GameObject hand;
    [Range(0, 1)] public float actionDelay;

    [Header("Furniture Placement")]
    [Range(1, 20)] public float placingRange;
    public float currentRange;
    public Material redMaterial;

    [Header("GUI")]
    public Image crosshairObject;
    public Sprite defaultCrosshair;

	#region Privates
	//Aux
	private Animator handAnimator;
    private float actionTimer;
    private SmartObject smart;
	public List<SmartObject> smartBehaviours;
    private bool canInteract;
    private RaycastHit hit;
    private Vector3 hitPoint;
    private GameObject placingObject;
    private HandManager handManager;
    //private Material objectMaterial;
    private float turnObjectTimer;
    private float turnObjectAngle;
    private string interactButton = "Fire1";
    private bool gridPlacement;
	#endregion

	void Start()
    {
        crosshairObject.sprite = defaultCrosshair;
        handAnimator = hand.GetComponent<Animator>();
        handManager = hand.GetComponent<HandManager>();
    }

    void Update()
    {
        Tools();

        HandHelper();
        Interact();
        UseItem();
    }

    private void HandHelper()
	{
        // Range
        if (handManager.handItem.itemData)
        {
            if (handManager.handItem.itemData.itemType == ItemData.ItemType.Decoration) currentRange = 9999;
            else currentRange = interactRange;
        }

        // Animation
        if (actionTimer <= 0
            && !handAnimator.GetCurrentAnimatorStateInfo(0).IsName("HandAction"))
        {
            if (hit.collider?.GetComponent<AiAgent>())
            {
                if (Input.GetButtonDown("Fire1") && handManager.handItem.itemData != null)
                    handAnimator.SetTrigger(handManager.handItem.itemData.animation.ToString());
            }
            else
            {
                if (Input.GetButton("Fire1") && handManager.handItem.itemData != null)
                    handAnimator.SetTrigger(handManager.handItem.itemData.animation.ToString());
            }
        }
	}

    private void Interact()
    {
        // If cursor isn't showing
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            actionTimer -= Time.deltaTime;

            // Raycast man
            if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, currentRange))
            {
                // If colliding with a smart object
                smartBehaviours = new List<SmartObject>(hit.collider?.GetComponents<SmartObject>());

                // Interacting with each smart property
                foreach (SmartObject smart in smartBehaviours)
                {
                    if (smart.GetObjectType() == SmartObject.ObjectType.CraftTool)
                    {
                        // Check button
                        if (Input.GetButton(smart.GetInteractButton())
                            && !handAnimator.GetCurrentAnimatorStateInfo(0).IsName("HandAction"))
                        {
                            // Interaction functions
                            smart.Interact();
                            StartCoroutine(InteractFeedback(smart.gameObject));

                            if (smart.particle)
                                Instantiate(smart.particle, hit.point, new Quaternion()).transform.LookAt(transform.position);
                        }

                        if (smart.GetComponent<GasToolObject>())
                        {
                            GasToolObject gastTool = smart.GetComponent<GasToolObject>();
                            gastTool.gui.SetActive(true);

                            if (Input.GetButtonDown("Fire1"))
                            {
                                if (handManager.handItem.itemData.gasTypes.Contains(
                                    gastTool.gasType))
                                {
                                    gastTool.AddGas(handManager.handItem.itemData.gasValue);
                                    handManager.RemoveItem(handManager.handItem);
                                }
                            }
                        }
                    }
                    else
                    {
                        // If interact delay have finished
                        if (actionTimer <= 0 && smart.CanInteract(hand))
                        {
                            // Check button
                            if (Input.GetButton(smart.GetInteractButton())
                                && !handAnimator.GetCurrentAnimatorStateInfo(0).IsName("HandAction"))
                            {
                                actionTimer = actionDelay;

                                // Interaction functions
                                smart.Interact();
                                StartCoroutine(InteractFeedback(smart.gameObject));

                                if (smart.particle)
                                    Instantiate(smart.particle, hit.point, new Quaternion()).transform.LookAt(transform.position);

                                if (smart.GetObjectType() == SmartObject.ObjectType.Chop)
                                    handManager.handItem.RemoveDurability(handManager);
                            }
                        }
                    }
                }
            }
        }
    }

    #region Items
    float foodTimer; 
    private DecorationObject furniture = null;
    private void UseItem()
    {
        GetComponent<FirstPersonController>().isEating = false;
        if (handManager.handItem.itemData)
        {
            switch (handManager.handItem.itemData.ability)
            {
				#region Furniture
				case ItemData.Ability.Placeable:
                    turnObjectTimer -= Time.deltaTime;
                    // If exists a placing object, destroy it
                    if (placingObject)
                    {
                        if (placingObject.name != handManager.handItem.itemData.itemName.english)
                            Destroy(placingObject);
                    }

                    // Load the furniture prefab
                    GameObject prefab = Resources.Load<GameObject>("Objects/Furnitures/" + handManager.handItem.itemData.itemName.english);

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

                        furniture = placingObject.GetComponent<DecorationObject>();
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
				case ItemData.Ability.Consumable:
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
                if (handManager.handItem.itemData.itemType != ItemData.ItemType.Decoration)
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

        if (obj) obj.transform.localScale = scale * 0.98f;
        if (obj) yield return new WaitForSeconds(0.1f);
        if (obj) obj.transform.localScale = scale;
    }
}
