using Assets.Scripts.Enums;
using Assets.Scripts.MiniGames.Balancing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;
    private BalancingGround balancingGround;
    private bool isRestarting = false;

    public float MovementVelocity => movementVelocity;
    public float RotationSpeed => rotationSpeed;

    public int DefaultStaticObjectDisplayOrder => defaultStaticObjectDisplayOrder;
    public int DefaultDraggingObjectDisplayOrder => defaultDraggingObjectDisplayOrder;

    private void Awake()
    {
        balancingGround = FindAnyObjectByType<BalancingGround>();
        balancingGround.OnBalancingObjectFall += BalancingGround_OnBalancingObjectFall;

        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);

        restartButton.onClick.AddListener(() => RestartGame());
        nextLevelButton.onClick.AddListener(() => RestartGame());
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

    public void RestartGame()
    {
        if (isRestarting)
        {
            return;
        }
        isRestarting = true;
        StartCoroutine(RestartWithDelay());
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

    private IEnumerator RestartWithDelay()
    {
        foreach (var balancingObject in balancingObjects)
        {
            balancingObject.enabled = false;
        }
        yield return new WaitForEndOfFrame();
        RestartImmediately();
    }

    private void RestartImmediately()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        if (isRestarting)
        {
            return;
        }
        towerObjects.Clear();
        BalancingObject currentObject = baseObject;
        BalancingObject previousObject = baseObject;
        int currentObjectDisplayOrder = baseObjectDisplayOrder;
        RecalculateTowerPart(currentObject, previousObject, currentObjectDisplayOrder);
    }

    private void RecalculateTowerPart(BalancingObject currentObject, BalancingObject previousObject, int currentObjectDisplayOrder)
    {
        towerObjects.Add(currentObject);
        currentObject.SetDisplayOrder(currentObjectDisplayOrder);
        currentObjectDisplayOrder--;
        var otherConnectedObjects = currentObject.ConnectedObjects.Where(co => co != previousObject);
        foreach (var connectedObject in otherConnectedObjects)
        {
            if (connectedObject.transform.position.y > currentObject.transform.position.y)
            {
                RecalculateTowerPart(connectedObject, currentObject, currentObjectDisplayOrder);
            }
        }
    }
}
