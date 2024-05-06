using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Assets.Scripts.Consts.AnimationConsts;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] private string displayingText;
    [SerializeField] private float interactionsDisplayTime = 2f;
    [SerializeField] private InventoryItem itemPrefab;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private List<InteractionTypeEnum> allowedInteractions;

    [SerializeField] private InteractionButton watchingButton;
    [SerializeField] private InteractionButton usageButton;
    [SerializeField] private InteractionButton takingButton;

    private Player player;
    private NarrativePanel narrativePanel;

    public IReadOnlyList<InteractionTypeEnum> AllowedInteractions => allowedInteractions;
    public InventoryItem ItemPrefab => itemPrefab;

    private float interactionsDisplayTimeLeft;
    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
        narrativePanel = FindAnyObjectByType<NarrativePanel>();

        HighlightOff();
        InitInteractionButtons();
        HideInteractions();
    }

    private void Update()
    {
        interactionsDisplayTimeLeft = Math.Max(0, interactionsDisplayTimeLeft - Time.deltaTime);
        if (interactionsDisplayTimeLeft <= 0)
        {
            HideInteractions();
        }
    }

    private void InitInteractionButtons()
    {
        watchingButton.Init(this, InteractionTypeEnum.Watching);
        takingButton.Init(this, InteractionTypeEnum.Taking);
        usageButton.Init(this, InteractionTypeEnum.Usage);

        watchingButton.OnInteracted += WatchingButton_OnInteracted;
        takingButton.OnInteracted += TakingButton_OnInteracted;
        usageButton.OnInteracted += UsageButton_OnInteracted;
    }

    private void UsageButton_OnInteracted(object sender, EventArgs e)
    {
        BeingUsed();
        HideInteractions();
    }

    private void TakingButton_OnInteracted(object sender, EventArgs e)
    {
        BeingTaken();
        HideInteractions();
    }

    private void WatchingButton_OnInteracted(object sender, EventArgs e)
    {
        BeingWatched();
        HideInteractions();
    }

    private void HideInteractions()
    {
        watchingButton.HideIcon();
        usageButton.HideIcon();
        takingButton.HideIcon();
    }

    public void ShowInteractions()
    {
        interactionsDisplayTimeLeft = interactionsDisplayTime;
        if (allowedInteractions.Contains(InteractionTypeEnum.Watching))
        {
            watchingButton.ShowIcon();
        }
        if (allowedInteractions.Contains(InteractionTypeEnum.Taking))
        {
            takingButton.ShowIcon();
        }
        if (allowedInteractions.Contains(InteractionTypeEnum.Usage))
        {
            usageButton.ShowIcon();
        }
    }

    public void InteractAs(InteractionTypeEnum interactableType)
    {
        switch (interactableType)
        {
            case InteractionTypeEnum.Watching:
                {
                    BeingWatched();
                }
                break;
            case InteractionTypeEnum.Taking:
                {
                    BeingTaken();
                }
                break;
            case InteractionTypeEnum.Usage:
                {
                    BeingUsed();
                }
                break;
        }
    }

    public void BeingWatched()
    {
        narrativePanel.ShowNewText(displayingText);
        Debug.Log($"{gameObject.name} being watched");
    }
    public void BeingTaken()
    {
        player.AddItemToInventory(ItemPrefab);
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
