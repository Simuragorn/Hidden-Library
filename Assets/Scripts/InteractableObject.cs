using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour
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
    private void OnMouseExit()
    {
        HighlightOff();
    }
}
