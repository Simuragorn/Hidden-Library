using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerEmitter2D : MonoBehaviour
{
    private Collider2D collider;
    public event EventHandler<Collider2D> OnTriggerEnter;
    public event EventHandler<Collider2D> OnTriggerExit;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter?.Invoke(this, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit?.Invoke(this, collision);
    }
}
