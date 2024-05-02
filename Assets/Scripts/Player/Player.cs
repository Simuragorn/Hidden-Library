using Assets.Scripts.Consts;
using Assets.Scripts.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
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
}
