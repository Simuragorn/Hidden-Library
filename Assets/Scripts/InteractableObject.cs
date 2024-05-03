using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private List<InteractableObjectTypeEnum> allowedInteractions;

    public IReadOnlyList<InteractableObjectTypeEnum> AllowedInteractions => allowedInteractions;
    public InventoryItem ItemPrefab => itemPrefab;
    private void Awake()
    {
        HighlightOff();
    }

    public void InteractAs(InteractableObjectTypeEnum interactableType)
    {
        switch (interactableType)
        {
            case InteractableObjectTypeEnum.Watchable:
                {
                    BeingWatched();
                }
                break;
            case InteractableObjectTypeEnum.Takable:
                {
                    BeingTaken();
                }
                break;
            case InteractableObjectTypeEnum.Usable:
                {
                    BeingUsed();
                }
                break;
        }
    }

    public void BeingWatched()
    {
        Debug.Log($"{gameObject.name} being watched");
    }
    public void BeingTaken()
    {
        Debug.Log($"{gameObject.name} being taken");
        Destroy(gameObject);
    }
    public void BeingUsed()
    {
        Debug.Log($"{gameObject.name} being used");
        Destroy(gameObject);
    }

    public void HighlightOn()
    {
        highlightObject.gameObject.SetActive(true);
    }
    private void HighlightOff()
    {
        highlightObject.gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HighlightOff();
    }
}
