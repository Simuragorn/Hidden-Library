using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private KeyCode fireKey;
    void Update()
    {
        if (Input.GetKeyDown(fireKey))
        {
            Vector2 spawnPosition = GetMousePosition();
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }

    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
