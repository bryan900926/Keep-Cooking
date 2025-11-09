using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// control the movement of the chef
public class Cooker : MonoBehaviour
{
    [SerializeField] private WorkerData workerData;

    [Header("Runtime References")]
    private SpriteRenderer spriteRenderer;
    private AIDestinationSetter destinationSetter;

    [SerializeField] private Transform destination;
    [SerializeField] private int cookIdx = -2;  // -2: waiting init, -1: quit job

    // 🔹 Public accessors
    public WorkerData WorkerData { get => workerData; set => workerData = value; }
    public Transform Destination => destination;
    public int CookIdx => cookIdx;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        destinationSetter = GetComponent<AIDestinationSetter>();

        if (workerData != null)
        {
            spriteRenderer.sprite = workerData.image;
        }
        else
        {
            Debug.LogWarning($"{name} has no WorkerData assigned!");
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // already has a destination → don’t recalc
        if (destination != null) return;

        // if assigned to a cooking spot
        if (cookIdx >= 0)
        {
            GameObject[] cookers = BackControl.Instance.GetCookers;
            if (cookIdx < cookers.Length)
            {
                destination = cookers[cookIdx].GetComponent<CookingSpot>().GetSpot;
                destinationSetter.target = destination;
            }
            else
            {
                Debug.LogWarning($"{name}: cookIdx {cookIdx} out of range!");
            }
        }
    }

    public void HandleQuitJob()
    {
        if (destination != null)
        {
            if (Vector2.Distance(transform.position, destination.position) <= 1f)
            {
                Dictionary<int, int> mapper = BackControl.Instance.Mapper;
                if (mapper.TryGetValue(cookIdx, out int controllUiIdx))
                {
                    mapper.Remove(cookIdx);
                    BackWorkerUIManager.Instance.RemoveWorkerInfoUI(controllUiIdx);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("worker cannot quit the job");
                }
            }
        }
    }

    // 🔹 API
    public void SetCookIdx(int idx) => cookIdx = idx;
    public void SetDestination(Transform target)
    {
        destination = target;
        if (destinationSetter != null) destinationSetter.target = target;
    }
}
