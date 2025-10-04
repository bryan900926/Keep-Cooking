using System.Collections;
using UnityEngine;

public class Chef : MonoBehaviour
{
    [Header("Cooking State")]
    private int dishIdx = -1;
    private float cookingTime = 0;
    private Cooker cooker;

    public int GetDishIdx => dishIdx;

    [Header("Energy")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float energyDecay = 3f;
    [SerializeField] private FloatingEnergyBar floatingEnergyBar;
    private float currentEnergy;
    [Header("Misbehave")]
    [SerializeField] private GameObject leaveTarget;   // where chef quit when exhausted
    private GameObject fires;

    private bool isQuit = false;


    private void Start()
    {
        currentEnergy = maxEnergy;
        cooker = GetComponent<Cooker>();
        GameObject cookingMachine = BackControl.Instance.GetCookers[cooker.CookIdx]; // GET THE cooking stove the worker use 
        foreach (Transform child in cookingMachine.transform)
        {
            if (child.name == "Fires")
            {
                fires = child.gameObject;
            }
        }
        if (!fires)
        {
            Debug.LogError("cannot find the fires gameobj");
        }
        fires.SetActive(false);
    }

    private void Update()
    {
        currentEnergy -= Time.deltaTime * energyDecay;
        HandleCooking();
        HandleEnergyDrain();
        floatingEnergyBar.UpdateEnergy(currentEnergy / maxEnergy);
    }

    public float EnableCooking(int foodIdx)
    {
        if (dishIdx != -1 || currentEnergy <= 0) return -1f;

        dishIdx = foodIdx;
        cookingTime = Random.Range(3f, 5f);
        return cookingTime;
    }

    private void HandleCooking()
    {
        if (dishIdx == -1) return;

        cookingTime -= Time.deltaTime;
        if (cookingTime <= 0f)
        {
            CreateDish();
        }
    }

    private void HandleEnergyDrain()
    {
        if (isQuit) return;

        currentEnergy -= Time.deltaTime;
        if (currentEnergy <= 0f)
        {
            currentEnergy = 0f;
            isQuit = true;
            HandleExhaustion();
        }
    }

    private void CreateDish()
    {
        var menu = Menu.Instance.FoodPrefabs;
        if (dishIdx != -1 && dishIdx < menu.Length && cooker.CookIdx != -1)
        {
            float wrongProb = GetWrongDishProb(currentEnergy);
            if (Random.value < wrongProb)
            {
                dishIdx = Random.Range(0, menu.Length);
                SetCookerOnFire();
            }

            var dishPrefab = menu[dishIdx];
            Vector2 spawnPos = (Vector2)transform.position + Vector2.right; // cleaner than new Vector2(1f,0f)
            Instantiate(dishPrefab, spawnPos, Quaternion.identity);

            dishIdx = -1;
        }
    }

    private float GetWrongDishProb(float energy)
    {
        return Mathf.Clamp01(1 - energy / maxEnergy);
    }

    private void HandleExhaustion()
    {
        if (cooker != null)
        {
            cooker.SetDestination(leaveTarget.transform);
            StartCoroutine(QuitWhenArrived());
        }

    }

    private IEnumerator QuitWhenArrived()
    {
        // Wait until the chef is close enough
        while (Vector2.Distance(transform.position, leaveTarget.transform.position) > 0.1f)
        {
            yield return null; // wait for next frame
        }

        cooker.HandleQuitJob();
    }

    private void SetCookerOnFire()
    {
        fires.SetActive(true);

    }
}
