using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] private GameObject highlightObject;
    private void Awake()
    {
        HighlightOff();
    }

    public void Interact(Player player)
    {
        Debug.Log($"Interacted on {gameObject.name}");
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
