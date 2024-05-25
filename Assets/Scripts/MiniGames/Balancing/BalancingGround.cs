using Assets.Scripts.MiniGames.Balancing;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class BalancingGround : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var balancingObject = collision.gameObject.GetComponent<BalancingObject>();
        if (balancingObject != null && balancingObject.IsTouched && !balancingObject.IsDragging)
        {
            Debug.Log("Defeat!");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
