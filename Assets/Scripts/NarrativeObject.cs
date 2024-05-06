using Assets.Scripts.Consts;
using UnityEngine;
using static Assets.Scripts.Consts.AnimationConsts;

[RequireComponent(typeof(Collider2D))]
public class NarrativeObject : MonoBehaviour
{
    [SerializeField] InteractionButton watchingButton;
    [SerializeField] private string displayingText;
    private NarrativePanel narrativePanel;

    private void Awake()
    {
        narrativePanel = FindObjectOfType<NarrativePanel>();
    }

    private void Start()
    {
        watchingButton.OnInteracted += WatchingButton_OnInteracted;
        watchingButton.HideIcon();
    }

    private void WatchingButton_OnInteracted(object sender, System.EventArgs e)
    {
        narrativePanel.ShowNewText(displayingText);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagConsts.Player))
        {
            watchingButton.ShowIcon();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagConsts.Player))
        {
            watchingButton.HideIcon();
        }
    }
}
