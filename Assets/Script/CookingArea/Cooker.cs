using Pathfinding;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    [SerializeField] private float smoothness = 5f;
    [SerializeField] private WorkerData workerData;

    public WorkerData GetWorkerData()
    {
        return workerData;
    }

    private SpriteRenderer spriteRenderer;
    private AIDestinationSetter desSetter;

    private Transform destination;

    public Transform Destination => destination;

    private int cookIdx = -1;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        desSetter = GetComponent<AIDestinationSetter>();

        spriteRenderer.sprite = workerData.image;


    }

    void Update()
    {
        TryMoveToCookingSpot();

    }

    private void TryMoveToCookingSpot()
    {
        if (destination) return;

        if (cookIdx != -1)
        {
            GameObject[] cookers = BackControl.Instance.GetCookers;
            destination = cookers[cookIdx].GetComponent<CookingSpot>().GetSpot;
            desSetter.target = destination;
        }

    }

    public void SetCookIdx(int idx)
    {
        cookIdx = idx;
    }

}
