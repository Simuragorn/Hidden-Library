using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CupsManager : MonoBehaviour
{
    [SerializeField] private CupObject cupPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    private List<CupObject> cups = new List<CupObject>();
    void Update()
    {
        HandleSpawn();
        HandleClear();
        HandleDisplayingOrder();
    }

    private void HandleDisplayingOrder()
    {
        cups = cups.OrderBy(c => c.transform.position.y).ToList();
        int initDisplayOrder = 1;
        for (int i = 0; i < cups.Count; i++)
        {
            int displayOrder = initDisplayOrder + i;
            CupObject cup = cups[i];
            cup.SetDisplayOrder(displayOrder);
        }
    }

    private void HandleClear()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            foreach (var cup in cups)
            {
                Destroy(cup.gameObject);
            }
            cups.Clear();
        }
    }

    private void HandleSpawn()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        if (Input.GetKeyDown(KeyCode.E))
        {
            spawnPoint = spawnPoints[0];
            CupObject newCup = Instantiate(cupPrefab, spawnPoint.position, Quaternion.identity);
            cups.Add(newCup);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            CupObject newCup = Instantiate(cupPrefab, spawnPoint.position, Quaternion.identity);
            cups.Add(newCup);
        }
    }
}
