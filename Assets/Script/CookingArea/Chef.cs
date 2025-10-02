using UnityEngine;

public class Chef : MonoBehaviour
{
    private int dishIdx = -1;
    private float cookingTime = 0;

    private float energy = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dishIdx != -1)
        {
            cookingTime -= Time.deltaTime;
            if (cookingTime <= 0)
            {
                CreateDish();
            }
        }

    }

    public void EnableCooking(int foodIdx)
    {
        if (dishIdx != -1) return;
        dishIdx = foodIdx;
        cookingTime = Random.Range(3, 5);

    }

    void CreateDish()
    {
        var menu = Menu.Instance.FoodPrefabs;
        if (dishIdx != -1 && dishIdx < menu.Length)
        {
            var dishes = Menu.Instance.FoodPrefabs[dishIdx];
            Vector2 spawnPos = (Vector2)transform.position + new Vector2(1f, 0f); // 1 unit right
            Instantiate(dishes, spawnPos, Quaternion.identity);
            dishIdx = -1;
        }
    }


}
