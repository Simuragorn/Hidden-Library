using Assets.Scripts.Enums;
using Assets.Scripts.MiniGames.Balancing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalancingManager : MonoBehaviour
{
    [SerializeField] private float attentionHintAngle = 20;
    [SerializeField] private float movementVelocity = 15f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private bool useJoints = true;
    [SerializeField] private int baseObjectDisplayOrder = 15;
    [SerializeField] private int defaultStaticObjectDisplayOrder = 5;
    [SerializeField] private int defaultDraggingObjectDisplayOrder = 10;

    [SerializeField] private BalancingObject baseObject;

    [SerializeField] private List<BalancingObject> balancingObjects = new();
    [SerializeField] private List<BalancingObject> towerObjects = new();
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    private BalancingGround balancingGround;

    public float MovementVelocity => movementVelocity;
    public float RotationSpeed => rotationSpeed;

    public int DefaultStaticObjectDisplayOrder => defaultStaticObjectDisplayOrder;
    public int DefaultDraggingObjectDisplayOrder => defaultDraggingObjectDisplayOrder;

    private void Awake()
    {
        balancingGround = FindAnyObjectByType<BalancingGround>();
        balancingGround.OnBalancingObjectFall += BalancingGround_OnBalancingObjectFall;

        victoryPanel.gameObject.SetActive(false);
        defeatPanel.gameObject.SetActive(false);
    }

    private void BalancingGround_OnBalancingObjectFall(object sender, System.EventArgs e)
    {
        defeatPanel.gameObject.SetActive(true);
    }

    private void Start()
    {
        baseObject.SetAsBalancingBaseObject();
        RecalculateTower();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        CheckVictory();
    }

    private void CheckVictory()
    {
        if (towerObjects.Count == balancingObjects.Count)
        {
            foreach (var balancingObject in balancingObjects)
            {
                balancingObject.DisablePhysics();
            }
            victoryPanel.gameObject.SetActive(true);
        }
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
            if (connectedObject.ConnectedObjects.Where(co => co != previousObject).Count() > 1)
            {
                Debug.LogError("Multiple collisions in tower forbidden!");
                return;
            }
            connectedObject = connectedObject.ConnectedObjects.FirstOrDefault(co => co != previousObject);
            previousObject = tmpObject;
        }
    }
}
