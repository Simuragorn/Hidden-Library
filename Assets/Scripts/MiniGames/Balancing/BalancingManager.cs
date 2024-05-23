using Assets.Scripts.Enums;
using Assets.Scripts.MiniGames.Balancing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalancingManager : MonoBehaviour
{
    [SerializeField] private float attentionHintAngle = 20;
    [SerializeField] private float movementVelocity = 15f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private bool useJoints = true;

    [SerializeField] private List<BalancingObject> balancingObjectPrefabs;

    [SerializeField] private GameObject leftUserHint;
    [SerializeField] private GameObject rightUserHint;


    private List<BalancingObject> balancingObjects = new();
    private BalancingBasisObject basisObject;

    public float MovementVelocity => movementVelocity;
    public float RotationSpeed => rotationSpeed;

    private void Start()
    {
        basisObject = FindObjectOfType<BalancingBasisObject>();
    }

    void Update()
    {
        basisObject.SetMovementData(movementVelocity, rotationSpeed);
        HandleSpawn();
        HandleClear();
        HandleUserHint();
    }

    private void HandleUserHint()
    {
        float towerRotationAngle = 0;
        if (balancingObjects.Any())
        {
            BalancingObject lastObject = balancingObjects.Last();
            towerRotationAngle = lastObject.transform.rotation.eulerAngles.z;
        }
        float absoluteAngle = towerRotationAngle > 180 ? 360 - towerRotationAngle : towerRotationAngle;
        bool showAttentionHint = absoluteAngle > attentionHintAngle;
        rightUserHint.SetActive(towerRotationAngle > 180 && showAttentionHint);
        leftUserHint.SetActive(towerRotationAngle < 180 && showAttentionHint);
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
        BalancingObject previousObject = basisObject;

        if (balancingObjects.Any())
        {
            previousObject = balancingObjects.Last();
        }
        IReadOnlyList<BalancingObjectPosition> availableObjectPositions = previousObject.BalancingObjectPositions;
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

        int displayOrder = previousObject.DisplayOrder - 1;
        Vector2 position = (Vector2)previousObject.transform.TransformPoint((Vector2)suitableObjectPosition.LocalPosition);
        Quaternion rotation = previousObject.transform.rotation * Quaternion.Euler(suitableObjectPosition.LocalRotation);
        BalancingObject newBalancingObject = Instantiate(suitablePrefab, position, rotation);
        Rigidbody2D connectedRigidbody = (previousObject is BalancingBasisObject) ? null : previousObject.Rigidbody;
        if (!useJoints)
        {
            connectedRigidbody = null;
        }
        newBalancingObject.Init(displayOrder, connectedRigidbody);
        balancingObjects.Add(newBalancingObject);
    }
}
