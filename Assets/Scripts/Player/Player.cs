using Assets.Scripts.Consts;
using Assets.Scripts.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject VFX;
    private PlayerMovement playerMovement;
    private RouteManager routeManager;
    private InteractableObject interactableTarget;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        routeManager = FindAnyObjectByType<RouteManager>();
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
            interactableTarget.Interact(this);
            interactableTarget = null;
            playerMovement.ResetTarget();
        }
    }

    private void HandleNewMovementTarget()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            interactableTarget = null;
        }
        if (!IsMouseOnScreen())
        {
            return;
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
