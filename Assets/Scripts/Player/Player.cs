using Assets.Scripts.Dto;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private RouteManager routeManager;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        routeManager = FindAnyObjectByType<RouteManager>();
    }


    private void Update()
    {
        HandleNewTarget();
        HandleInteraction();
    }

    private void HandleNewTarget()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
        {
            return;
        }
        Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        BuildedRoute buildedRoute = routeManager.BuildRoute(transform.position, mouseWorldPoint);
        if (buildedRoute == null)
        {
            playerMovement.ResetTarget();
        }
        else if (buildedRoute.Route == null)
        {
            playerMovement.SetDirectTarget(mouseWorldPoint);
        }
        else
        {
            playerMovement.SetBuildedRoute(buildedRoute);
        }
    }

    private void HandleInteraction()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
        {
            return;
        }

        Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPoint, Vector2.zero);
        if (hits.Length > 0)
        {
            List<InteractableObject> interactableObjects = hits.Select(h => h.collider.GetComponent<InteractableObject>()).Where(c => c != null).ToList();
            foreach (InteractableObject interactableObject in interactableObjects)
            {
                Debug.Log($"Clicked on {interactableObject.gameObject.name}");
            }
        }
    }
}
