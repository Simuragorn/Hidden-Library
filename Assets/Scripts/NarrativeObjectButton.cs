using Assets.Scripts.Consts;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NarrativeObjectButton : MonoBehaviour
{
    private bool available = false;
    private Animator animator;
    private NarrativePanel narrativePanel;
    private string displayingText;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        narrativePanel = FindObjectOfType<NarrativePanel>();
    }

    public void Init(string text)
    {
        displayingText = text;
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
        narrativePanel.ShowNewText(displayingText);
        HideIcon();
    }
}
