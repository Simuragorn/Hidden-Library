using Assets.Scripts.Consts;
using Assets.Scripts.Enums;
using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class InteractionButton : MonoBehaviour
{
    public event EventHandler OnInteracted;
    public InteractionTypeEnum InteractionType {  get; private set; }
    public InteractableObject InteractableObject {  get; private set; }
    protected bool available = false;
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Init(InteractableObject interactableObject, InteractionTypeEnum interactionType)
    {
        InteractionType = interactionType;
        InteractableObject = interactableObject;
    }

    public void ShowIcon()
    {
        available = true;
        animator.SetInteger(AnimationConsts.AnimationStateKey, AnimationConsts.NarrativeIcon.ShowIconValue);
    }

    public void HideIcon()
    {
        available = false;
        animator.SetInteger(AnimationConsts.AnimationStateKey, AnimationConsts.NarrativeIcon.HideIconValue);
    }

    protected virtual void OnMouseDown()
    {
        if (!available)
        {
            return;
        }
        OnInteracted?.Invoke(this, EventArgs.Empty);
        HideIcon();
    }
}
