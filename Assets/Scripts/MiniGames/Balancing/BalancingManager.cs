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

    [SerializeField] private GameObject leftUserHint;
    [SerializeField] private GameObject rightUserHint;


    private List<BalancingObject> balancingObjects = new();

    public float MovementVelocity => movementVelocity;
    public float RotationSpeed => rotationSpeed;

    void Update()
    {
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
}
