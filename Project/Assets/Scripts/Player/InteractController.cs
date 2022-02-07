using System;
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
    public Material placingMaterial;
    public Material redMaterial;

	#region Privates
	//Aux
	private Animator handAnimator;
    private float actionTimer;
	public List<SmartObject> smartBehaviours;
    private RaycastHit hit;
    private Vector3 hitPoint;
    public GameObject placingObject;
    private HandManager handManager;
    //private Material objectMaterial;
    private float turnObjectTimer;
    private float turnObjectAngle;
    //private string interactButton = "Fire1";
    private bool gridPlacement;
	#endregion

	void Start()
    {
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

            bool ray;
            LayerMask mask = LayerMask.GetMask("Ignore Raycast", "SnapPoint"); 
            if (placingObject != null)
			{
                if (placingObject.GetComponent<DecorationObject>().snap == DecorationObject.SnapType.None)
				{
                    
                    ray = Physics.Raycast(head.transform.position, head.transform.forward, out hit, currentRange, ~mask);
                }
				else
				{
                    ray = Physics.Raycast(head.transform.position, head.transform.forward, out hit, currentRange);
                }
            }
			else
			{
                ray = Physics.Raycast(head.transform.position, head.transform.forward, out hit, currentRange, ~mask);
            }

            // Raycast
            if (ray)
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
                            //StartCoroutine(InteractFeedback(smart.gameObject));

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
                                StartCoroutine(InteractFeedback(smart.gameObject, 0.99f, 0.05f));

                                if (smart.particle)
                                    Instantiate(smart.particle, hit.point, new Quaternion()).transform.LookAt(transform.position);

                                if (smart.GetObjectType() == SmartObject.ObjectType.Chop)
                                {
                                    handManager.handItem.RemoveDurability(handManager);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #region Items
    float foodTimer; 
    private DecorationObject decorationObject = null;
    private void UseItem()
    {
        GetComponent<FirstPersonController>().isEating = false;
        if (handManager.handItem.itemData)
        {
            switch (handManager.handItem.itemData.ability)
            {
				#region Placeable
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
                        decorationObject = placingObject.GetComponent<DecorationObject>();
                        decorationObject.EnableTrigger(true);
                        decorationObject.redMaterial = redMaterial;
                        decorationObject.isPlacing = true;
                    }

                    // Controls
                    if (Input.GetButton("Turn") && turnObjectTimer <= 0)
                    {
                        turnObjectAngle += 90;
                        placingObject.transform.eulerAngles = new Vector3(0, turnObjectAngle, 0);
                        turnObjectTimer = actionDelay;

                        if (turnObjectAngle >= 360) turnObjectAngle = 0;
                    }
                    else if (Input.GetButtonDown("Turn"))
                    {
                        turnObjectAngle += 90;
                        placingObject.transform.eulerAngles = new Vector3(0, turnObjectAngle, 0);
                        turnObjectTimer = actionDelay / 2;
                        if (turnObjectAngle >= 360) turnObjectAngle = 0;
                    }
                    
                    // If placement furniture object exists
                    if (placingObject)
                    {
                        if (placingObject.GetComponent<DecorationObject>().snap != DecorationObject.SnapType.None)
                        {
                            // Placing object position
                            if (hit.collider)
                            {
                                if (hit.collider.gameObject.GetComponent<SnapPoint>())
                                {
                                    Renderer placingRenderer = placingObject.GetComponent<DecorationObject>().rend;
                                    if (placingRenderer == null) placingRenderer = placingObject.GetComponent<Renderer>();
                                    SnapPoint snapPoint = hit.collider.gameObject.GetComponent<SnapPoint>();
                                    float moveMod = 2;

                                    // Wall Snap
                                    if (decorationObject.snap == DecorationObject.SnapType.Wall)
                                    {
                                        SnapPoint.Position pos = snapPoint.GetPosition();
                                        if (snapPoint.transform.parent.GetComponent<DecorationObject>().snap == DecorationObject.SnapType.Ground)
                                            pos = SnapPoint.Position.Top;

                                        if (pos == SnapPoint.Position.Top)
                                        {
                                            decorationObject.transform.position = snapPoint.transform.position;
                                        }
                                        else if (pos == SnapPoint.Position.Bottom)
										{
                                            decorationObject.transform.position = snapPoint.transform.position
                                                - new Vector3(0, placingRenderer.bounds.size.y / moveMod, 0); ;
                                        }
                                        else
                                        {
                                            if (snapPoint.transform.GetComponentInParent<DecorationObject>().isHorizontal)
                                            {
                                                if (turnObjectAngle == 90)
                                                {
                                                    placingObject.transform.position = snapPoint.transform.position +
                                                            new Vector3(0, 0, placingRenderer.bounds.size.z / moveMod);
                                                }
                                                else if (turnObjectAngle == 270)
                                                {
                                                    placingObject.transform.position = snapPoint.transform.position -
                                                           new Vector3(0, 0, placingRenderer.bounds.size.z / moveMod);
                                                }
                                                else if (turnObjectAngle == 0 || turnObjectAngle == 180)
                                                {
                                                    if (snapPoint.GetPosition() == SnapPoint.Position.Right)
                                                    {
                                                        placingObject.transform.position = snapPoint.transform.position +
                                                                new Vector3(placingRenderer.bounds.size.x / moveMod, 0, 0);
                                                    }
                                                    else if (snapPoint.GetPosition() == SnapPoint.Position.Left)
                                                    {
                                                        placingObject.transform.position = snapPoint.transform.position -
                                                                new Vector3(placingRenderer.bounds.size.x / moveMod, 0, 0);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (turnObjectAngle == 0)
                                                {
                                                    placingObject.transform.position = snapPoint.transform.position +
                                                            new Vector3(placingRenderer.bounds.size.x / moveMod, 0, 0);
                                                }
                                                else if (turnObjectAngle == 180)
                                                {
                                                    placingObject.transform.position = snapPoint.transform.position -
                                                           new Vector3(placingRenderer.bounds.size.x / moveMod, 0, 0);
                                                }
                                                else if (turnObjectAngle == 90 || turnObjectAngle == 270)
                                                {
                                                    if (snapPoint.GetPosition() == SnapPoint.Position.Right)
                                                    {
                                                        placingObject.transform.position = snapPoint.transform.position -
                                                                new Vector3(0, 0, placingRenderer.bounds.size.z / moveMod);
                                                    }
                                                    else if (snapPoint.GetPosition() == SnapPoint.Position.Left)
                                                    {
                                                        placingObject.transform.position = snapPoint.transform.position +
                                                                new Vector3(0, 0, placingRenderer.bounds.size.z / moveMod);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    // Ground Snap
                                    else if (decorationObject.snap == DecorationObject.SnapType.Ground)
                                    {
                                        if (snapPoint.GetPosition() == SnapPoint.Position.Top)
                                        {
                                            placingObject.transform.position = snapPoint.transform.position +
                                                new Vector3(0, 0, placingRenderer.bounds.size.z / moveMod);
                                        }
                                        else if (snapPoint.GetPosition() == SnapPoint.Position.Bottom)
                                        {
                                            placingObject.transform.position = snapPoint.transform.position -
                                                new Vector3(0, 0, placingRenderer.bounds.size.z / moveMod);
                                        }
                                        else if (snapPoint.GetPosition() == SnapPoint.Position.Right)
                                        {
                                            placingObject.transform.position = snapPoint.transform.position +
                                                new Vector3(placingRenderer.bounds.size.x / moveMod, 0, 0);
                                        }
                                        else if (snapPoint.GetPosition() == SnapPoint.Position.Left)
                                        {
                                            placingObject.transform.position = snapPoint.transform.position -
                                                new Vector3(placingRenderer.bounds.size.x / moveMod, 0, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    if (gridPlacement)
                                        // Snap to grid
                                        placingObject.transform.position = new Vector3(
                                            (float)(Math.Round(hitPoint.x * 4, MidpointRounding.ToEven) / 4),
                                            (float)(Math.Round(hitPoint.y * 4, MidpointRounding.ToEven) / 4),
                                            (float)(Math.Round(hitPoint.z * 4, MidpointRounding.ToEven) / 4));
                                    //placingObject.transform.position = new Vector3((int)hitPoint.x, (int)hitPoint.y, (int)hitPoint.z);
                                    else
                                        placingObject.transform.position = hitPoint + (Vector3.up * 0.015f);
                                }
                            }
                        }
                        else
                        {
                            if (gridPlacement)
                                // Snap to grid
                                placingObject.transform.position = new Vector3(
                                    (float)(Math.Round(hitPoint.x * 4, MidpointRounding.ToEven) / 4),
                                    (float)(Math.Round(hitPoint.y * 4, MidpointRounding.ToEven) / 4),
                                    (float)(Math.Round(hitPoint.z * 4, MidpointRounding.ToEven) / 4));
                            //placingObject.transform.position = new Vector3((int)hitPoint.x, (int)hitPoint.y, (int)hitPoint.z);
                            else
                                placingObject.transform.position = hitPoint + (Vector3.up * 0.015f);
                        }

                        // Placement
                        if (Vector3.Distance(transform.position, placingObject.transform.position) < placingRange)
                        {
                            if (decorationObject != null)
                            {
                                // Overlapping materials
                                if (!decorationObject.isOverlapping)
                                    try
                                    {
                                        decorationObject.SetMaterial(placingMaterial);
                                        //placingObject.GetComponent<Renderer>().material = furniture.originalMaterial;
                                    }
                                    catch
                                    {
                                        Renderer[] rends = placingObject.GetComponentsInChildren<Renderer>();
                                        foreach (Renderer r in rends)
                                        {
                                            decorationObject.SetMaterial(placingMaterial);
                                            //r.material = furniture.originalMaterial;
                                        }
                                    }
                                else
                                    decorationObject.SetRed();
                                    //placingObject.GetComponent<Renderer>().material = redMaterial;

                                // Place item
                                if (Input.GetButtonDown("Fire1") && FindObjectOfType<UIManager>().state == UIManager.MenuState.None)
                                {
                                    if (!decorationObject.isOverlapping)
                                    {
                                        // Enable components
                                        decorationObject.PlaceObject();
                                        handManager.PlaceItem();
                                        placingObject = null;
                                        if (turnObjectAngle == 0 || turnObjectAngle == 180)
                                            decorationObject.isHorizontal = true;
                                        StartCoroutine(InteractFeedback(decorationObject.gameObject, 0.9f, 0.07f));
                                    }
                                }
                            }
                        }
                        else
                        {
                            // If far from player, red material to it
                            //placingObject.GetComponent<Renderer>().material = redMaterial;
                            placingObject.GetComponent<DecorationObject>().SetRed();
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

    private IEnumerator InteractFeedback(GameObject obj, float sizeMod, float delay)
    {
        Vector3 scale = obj.transform.localScale;

        if (obj) obj.transform.localScale = scale * sizeMod;
        if (obj) yield return new WaitForSeconds(delay);
        if (obj) obj.transform.localScale = scale;
    }
}
