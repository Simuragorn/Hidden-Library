using Assets.Scripts.Enums;
using Assets.Scripts.MiniGames.Balancing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BalancingManager : MonoBehaviour
{
    [SerializeField] private float victoryDelay = 5f;
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
    [SerializeField] private TextMeshProUGUI victoryDelayTextComponent;
    private BalancingGround balancingGround;
    private bool isGameFinished = false;
    private bool isRestarting = false;
    private bool isReadyForVictoryCheck = false;
    [SerializeField] private float victoryDelayLeft;

    public float MovementVelocity => movementVelocity;
    public float RotationSpeed => rotationSpeed;

    public int DefaultStaticObjectDisplayOrder => defaultStaticObjectDisplayOrder;
    public int DefaultDraggingObjectDisplayOrder => defaultDraggingObjectDisplayOrder;

    private void Awake()
    {
        victoryDelayLeft = victoryDelay;
        victoryDelayTextComponent.text = $"Соберите чашки";
        balancingGround = FindAnyObjectByType<BalancingGround>();
        balancingGround.OnBalancingObjectFall += BalancingGround_OnBalancingObjectFall;

        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);

        restartButton.onClick.AddListener(() => RestartGame());
        nextLevelButton.onClick.AddListener(() => RestartGame());
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            CheckTowerFinishing();
        }
        if (isReadyForVictoryCheck && !isGameFinished)
        {
            victoryDelayTextComponent.text = $"Удержите чашки ещё {(int)victoryDelayLeft} секунд";
            victoryDelayLeft -= Time.deltaTime;
            if (victoryDelayLeft <= 0)
            {
                ShowVictory();
            }
        }
    }

    private void BalancingGround_OnBalancingObjectFall(object sender, System.EventArgs e)
    {
        victoryDelayTextComponent.text = $"Башня развалилась...";
        isGameFinished = true;
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
        newBalancingObject.OnDragStarted += NewBalancingObject_OnDragStarted;
        balancingObjects.Add(newBalancingObject);
    }

    private void NewBalancingObject_OnDragStarted(object sender, BalancingObject e)
    {
        CheckTowerFinishing();
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
        CheckTowerFinishing();
    }

    private void ShowVictory()
    {
        isGameFinished = true;
        victoryDelayTextComponent.text = $"Даже не разбилось ничего";
        foreach (var balancingObject in balancingObjects)
        {
            balancingObject.DisablePhysics();
        }
        victoryPanel.gameObject.SetActive(true);
    }

    private void RecalculateTower()
    {
        if (isGameFinished)
        {
            return;
        }
        towerObjects.Clear();
        BalancingObject currentObject = baseObject;
        BalancingObject previousObject = baseObject;
        RecalculateTowerPart(currentObject, previousObject);
    }

    private void RecalculateTowerPart(BalancingObject currentObject, BalancingObject previousObject)
    {
        towerObjects.Add(currentObject);
        int displayOrder = baseObjectDisplayOrder;
        if (currentObject != baseObject)
        {
            float rotation = currentObject.transform.rotation.eulerAngles.z;
            int displayOffset = rotation > 90 && rotation < 270 ? 1 : -1;
            displayOrder = previousObject.DisplayOrder + displayOffset;
        }
        currentObject.SetDisplayOrder(displayOrder);

        var otherConnectedObjects = currentObject.ConnectedObjects.Where(co => co != previousObject && co.transform.position.y > currentObject.transform.position.y);
        var neededObject = otherConnectedObjects.OrderByDescending(co => co.ConnectedObjects.Count).FirstOrDefault();
        if (neededObject != null)
        {
            RecalculateTowerPart(neededObject, currentObject);
        }
    }

    private void CheckTowerFinishing()
    {
        isReadyForVictoryCheck = IsTowerFinished();
        if (!isReadyForVictoryCheck && victoryDelayLeft != victoryDelay)
        {
            victoryDelayLeft = victoryDelay;
            victoryDelayTextComponent.text = $"Соберите чашки";
        }
    }

    private bool IsTowerFinished()
    {
        if (towerObjects.Count == balancingObjects.Count)
        {
            if (balancingObjects.All(bo => !bo.IsDragging))
            {
                return true;
            }
        }
        return false;
    }
}
