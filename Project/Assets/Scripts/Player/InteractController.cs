using System.Collections;
using System.Collections.Generic;
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
    private Vector3 hitPoint;


    void Start()
    {
        crosshairObject.sprite = defaultCrosshair;
        handAnimator = hand.GetComponent<Animator>();
    }

    void Update()
    {
        InteractControl();
        UseItem();
    }

    private void UseItem()
    {
        HandManager handManager = hand.GetComponent<HandManager>();

        switch (handManager.handItemData.itemType)
        {
            case Item.ItemType.Furniture:
                handManager.handItemData.UseFurniture(handManager, handManager.handItem, hitPoint);
                break;
        }
    }

    private void InteractControl()
    { 
        if (Time.timeScale != 0)
        {
            actionTimer -= Time.deltaTime;

            if (Physics.Raycast(head.transform.position, head.transform.forward, out RaycastHit hit, interactRange))
            {
                Debug.Log(hit.collider.name);
                hitPoint = hit.point;
                if (hit.collider.GetComponent<SmartObject>())
                {
                    smart = hit.collider.GetComponent<SmartObject>();
                    if (smart.CanInteract(hand)) canInteract = true;
                    else canInteract = false;
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
                if (Input.GetButton("Fire1"))
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
}
