using Assets.Scripts.Consts;
using Assets.Scripts.Dto;
using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject VFX;
    private PlayerInventory playerInventory;
    private PlayerMovement playerMovement;
    private RouteManager routeManager;
    private InteractableObject interactableTarget;
    int UILayer;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInventory = GetComponent<PlayerInventory>();
        routeManager = FindAnyObjectByType<RouteManager>();

        UILayer = LayerMask.NameToLayer(LayerNameConsts.UI);
    }

    private void Update()
    {
        HandleNewMovementTarget();
        HandleInteraction();
        HandleInteractableTarget();
        HandleVFXFlip();
    }

    private void HandleVFXFlip()
    {
        float xMovementDirection = playerMovement.MovementDirection.x;
        if (xMovementDirection != 0)
        {
            Vector3 vfxScale = VFX.transform.localScale;
            vfxScale.x = xMovementDirection > 0 ? -1 : 1;
            VFX.transform.localScale = vfxScale;
        }
    }

    private void HandleInteractableTarget()
    {
        if (interactableTarget == null)
        {
            return;
        }
        float distanceToTarget = Vector2.Distance(interactableTarget.transform.position, transform.position);
        if (distanceToTarget < InteractableObjectConstants.MaxInteractionDistance)
        {
            TryInteract();
            interactableTarget = null;
            playerMovement.ResetTarget();
        }
    }

    private void TryInteract()
    {
        InteractableObjectTypeEnum interactionType = interactableTarget.AllowedInteractions.FirstOrDefault();
        Interact(interactionType);
    }

    private void Interact(InteractableObjectTypeEnum interactionType)
    {
        switch (interactionType)
        {
            case InteractableObjectTypeEnum.Watchable:
                {
                    Debug.Log($"Watching {interactableTarget.gameObject.name}");
                }
                break;
            case InteractableObjectTypeEnum.Takable:
                {
                    Debug.Log($"Taking {interactableTarget.gameObject.name}");
                    playerInventory.AddItem(interactableTarget.ItemPrefab);
                }
                break;
            case InteractableObjectTypeEnum.Usable:
                {
                    Debug.Log($"Using {interactableTarget.gameObject.name}");
                }
                break;
        }
        interactableTarget.InteractAs(interactionType);
    }

    private void HandleNewMovementTarget()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0) || IsPointerOverUIElement())
        {
            return;
        }
        Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TrySetNewTarget(mouseWorldPoint);
    }

    private void TrySetNewTarget(Vector2 targetPoint)
    {
        BuildedRoute buildedRoute = routeManager.BuildRoute(transform.position, targetPoint);
        if (buildedRoute == null)
        {
            playerMovement.ResetTarget();
        }
        else if (buildedRoute.Route == null)
        {
            playerMovement.SetDirectTarget(targetPoint);
        }
        else
        {
            playerMovement.SetBuildedRoute(buildedRoute);
        }
    }

    private void HandleInteraction()
    {
        if (!IsMouseOnScreen() || IsPointerOverUIElement())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            interactableTarget = null;
        }
        Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var interactableObjectLayer = LayerMask.GetMask(LayerNameConsts.InteractableObject);
        List<InteractableObject> interactableObjects = Physics2D.RaycastAll(mouseWorldPoint, Vector2.zero, 1f, interactableObjectLayer).Select(h => h.collider.GetComponent<InteractableObject>()).ToList();
        foreach (InteractableObject interactableObject in interactableObjects)
        {
            float distanceToObject = Vector2.Distance(interactableObject.transform.position, transform.position);
            if (distanceToObject < InteractableObjectConstants.MaxHighlightDistance)
            {
                interactableObject.HighlightOn();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    interactableTarget = interactableObject;
                    TrySetNewTarget(interactableTarget.transform.position);
                }
            }
        }
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public bool IsMouseOnScreen()
    {
#if UNITY_EDITOR
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Handles.GetMainGameViewSize().x - 1 || Input.mousePosition.y >= Handles.GetMainGameViewSize().y - 1)
        {
            return false;
        }
#else
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
        return false;
        }
#endif
        else
        {
            return true;
        }
    }
}
