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
    [SerializeField] private int baseObjectDisplayOrder = 15;
    [SerializeField] private int defaultObjectDisplayOrder = 5;

    [SerializeField] private BalancingObject baseObject;
    [SerializeField] private GameObject leftUserHint;
    [SerializeField] private GameObject rightUserHint;

    [SerializeField] private List<BalancingObject> balancingObjects = new();
    [SerializeField] private List<BalancingObject> towerObjects = new();

    public float MovementVelocity => movementVelocity;
    public float RotationSpeed => rotationSpeed;

    private void Start()
    {
        baseObject.SetAsBalancingBaseObject();
        RecalculateTower();
    }

    void Update()
    {
        HandleUserHint();
    }

    public void AddBalancingObject(BalancingObject newBalancingObject)
    {
        newBalancingObject.OnCollisionHappened += NewBalancingObject_OnCollisionHappened;
        balancingObjects.Add(newBalancingObject);
    }

    public void RemoveBalancingObject(BalancingObject balancingObject)
    {
        if (balancingObjects.Contains(balancingObject))
        {
            balancingObject.OnCollisionHappened -= NewBalancingObject_OnCollisionHappened;
            balancingObjects.Remove(balancingObject);
        }
    }

    private void NewBalancingObject_OnCollisionHappened(object sender, BalancingObject balancingObject)
    {
        RecalculateTower();
    }

    private void RecalculateTower()
    {
        towerObjects.Clear();
        BalancingObject connectedObject = baseObject;
        BalancingObject previousObject = baseObject;
        int currentObjectDisplayOrder = baseObjectDisplayOrder;
        while (connectedObject != null)
        {
            towerObjects.Add(connectedObject);
            connectedObject.SetDisplayOrder(currentObjectDisplayOrder);
            currentObjectDisplayOrder--;
            var tmpObject = connectedObject;
            connectedObject = connectedObject.ConnectedObjects.FirstOrDefault(co => co != previousObject);
            previousObject = tmpObject;
        }
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
}
