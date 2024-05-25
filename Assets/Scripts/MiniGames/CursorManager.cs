using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private bool allowMultipleDrag = false;
    private List<DragListener> dragListeners = new List<DragListener>();
    void Update()
    {
        DragObjects();
    }

    private void DragObjects()
    {
        bool dragging = Input.GetMouseButtonDown(0);
        if (!dragging)
        {
            return;
        }
        Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var interactableObjectLayer = LayerMask.GetMask(LayerNameConsts.InteractableObject);
        var hits = Physics2D.RaycastAll(mouseWorldPoint, Vector2.zero, 1f, interactableObjectLayer).ToList();
        List<DragListener> dragListeners = hits.Where(h => h.collider != null).Select(h => h.collider.GetComponent<DragListener>()).Where(e => e != null).ToList();
        dragListeners = dragListeners.OrderByDescending(o => o.DisplayOrder).ToList();
        foreach (var dragListener in dragListeners)
        {
            if (dragging)
            {
                dragListener.OnDrag();
                if (!allowMultipleDrag)
                {
                    return;
                }
            }
        }
    }
}
