using Assets.Scripts.Enums;
using Assets.Scripts.MiniGames.Balancing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalancingManager : MonoBehaviour
{
    [SerializeField] private List<BalancingObject> balancingObjectPrefabs;
    private List<BalancingObject> balancingObjects = new();
    private BalancingBasisObject basisObject;

    private void Start()
    {
        basisObject = FindObjectOfType<BalancingBasisObject>();
    }

    void Update()
    {
        HandleSpawn();
        HandleClear();
    }

    private void HandleClear()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            foreach (var cup in balancingObjects)
            {
                Destroy(cup.gameObject);
            }
            balancingObjects.Clear();
        }
    }

    private void HandleSpawn()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        Transform lowerBalancingObject = basisObject.transform;
        int displayOrder = basisObject.DisplayOrder - 1;
        IReadOnlyList<BalancingObjectPosition> availableObjectPositions = basisObject.BalancingObjectPositions;
        if (balancingObjects.Any())
        {
            availableObjectPositions = balancingObjects.Last().BalancingObjectPositions;
            lowerBalancingObject = balancingObjects.Last().transform;
            displayOrder = balancingObjects.Last().DisplayOrder - 1;
        }
        List<BalancingObjectTypeEnum> availableTypes = availableObjectPositions.Select(op => op.BalancingObjectType).ToList();
        List<BalancingObject> suitablePrefabs = balancingObjectPrefabs.Where(p => availableTypes.Contains(p.Type)).ToList();
        if (!suitablePrefabs.Any())
        {
            Debug.Log("NO SUITABLE BALANCING OBJECTS FOUND");
            return;
        }
        int objectIndex = Random.Range(0, suitablePrefabs.Count);
        var suitablePrefab = suitablePrefabs[objectIndex];
        var suitableObjectPosition = availableObjectPositions.First(p => p.BalancingObjectType == suitablePrefab.Type);


        Vector2 position = (Vector2)lowerBalancingObject.transform.TransformPoint((Vector2)suitableObjectPosition.LocalPosition);
        Quaternion rotation = lowerBalancingObject.transform.rotation * Quaternion.Euler(suitableObjectPosition.LocalRotation);
        BalancingObject newBalancingObject = Instantiate(suitablePrefab, position, rotation);
        newBalancingObject.SetDisplayOrder(displayOrder);
        balancingObjects.Add(newBalancingObject);
    }
}
