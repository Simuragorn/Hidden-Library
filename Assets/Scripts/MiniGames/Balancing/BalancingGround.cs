using Assets.Scripts.MiniGames.Balancing;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BalancingGround : MonoBehaviour
{
    public event EventHandler OnBalancingObjectFall;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var balancingObject = collision.gameObject.GetComponent<BalancingObject>();
        if (balancingObject != null)
        {
            OnBalancingObjectFall?.Invoke(this, EventArgs.Empty);
        }
    }
}
