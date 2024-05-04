using Assets.Scripts.Consts;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NarrativeObject : MonoBehaviour
{
    [SerializeField] NarrativeObjectButton narrativeButton;
    [SerializeField] private string displayingText;

    private void Start()
    {
        narrativeButton.Init(displayingText);
        narrativeButton.HideIcon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagConsts.Player))
        {
            narrativeButton.ShowIcon();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagConsts.Player))
        {
            narrativeButton.HideIcon();
        }
    }
}
