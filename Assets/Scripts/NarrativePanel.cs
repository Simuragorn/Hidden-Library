using Assets.Scripts.Consts;
using TMPro;
using UnityEngine;

public class NarrativePanel : MonoBehaviour
{
    [SerializeField] private float displayDelay = 2f;
    [SerializeField] private TextMeshProUGUI textComponent;
    private Animator animator;
    private float delayLeft = 0;
    private bool isDisplaying;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        HideText();
    }

    private void Update()
    {
        if (isDisplaying && delayLeft <= 0)
        {
            HideText();
        }
        delayLeft = Mathf.Max(0, delayLeft - Time.deltaTime);
    }

    public void ShowNewText(string displayingText)
    {
        isDisplaying = true;
        textComponent.text = displayingText;
        animator.SetInteger(AnimationConsts.AnimationStateKey, AnimationConsts.NarrativePanel.ShowPanelValue);        
        delayLeft = displayDelay;
    }

    private void HideText()
    {
        isDisplaying = false;
        animator.SetInteger(AnimationConsts.AnimationStateKey, AnimationConsts.NarrativePanel.HidePanelValue);
    }
}
