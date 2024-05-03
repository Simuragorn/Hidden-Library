using Assets.Scripts.Consts;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NarrativeObjectButton : MonoBehaviour
{
    private bool available = false;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    private void OnMouseDown()
    {
        if(!available)
        {
            return;
        }
        Debug.Log("Narrative");
        HideIcon();
    }
}
