using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowStockReminder : MonoBehaviour
{
    public static LowStockReminder Instance;

    [SerializeField] private GameObject[] lowStockSlots;

    readonly private Dictionary<int, int> lowStockIndices = new();
    [SerializeField] private List<IngredientData> ingredientDatas;
    readonly private Dictionary<int, IngredientData> ingredientDataDict = new();

    // Cache the Image components so we don't call GetComponent every time
    private Image[] slotImages;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        foreach (var ingredientData in ingredientDatas)
        {
            ingredientDataDict[ingredientData.Mask] = ingredientData;
        }

        // Initialize Array and Cache Images
        slotImages = new Image[lowStockSlots.Length];

        for (int i = 0; i < lowStockSlots.Length; i++)
        {
            // 1. Ensure the GameObject is ACTIVE (so the Grid Layout works)
            lowStockSlots[i].SetActive(true);

            // 2. Get the Image component
            if (lowStockSlots[i].TryGetComponent<Image>(out var img))
            {
                slotImages[i] = img;
                // 3. Disable the IMAGE (so it is invisible but takes up space)
                img.enabled = false;
            }
            else
            {
                Debug.LogError($"Slot {i} is missing an Image component!");
            }
        }
    }

    public void AddLowStockIngredient(int ingredientIdx)
    {
        if (!lowStockIndices.ContainsKey(ingredientIdx))
        {
            int slotIdx = FetchSlotIndex();

            if (slotIdx == -1)
            {
                Debug.LogWarning("No available slot for low stock reminder!");
                return;
            }

            lowStockIndices.Add(ingredientIdx, slotIdx);
            IngredientData ingredientData = ingredientDataDict[ingredientIdx];

            // Enable the image
            if (slotImages[slotIdx] != null)
            {
                slotImages[slotIdx].sprite = ingredientData.image;

                // Reset Color (Fix transparency issues)
                var col = slotImages[slotIdx].color;
                col.a = 1f;
                slotImages[slotIdx].color = col;

                // TURN ON THE IMAGE
                slotImages[slotIdx].enabled = true;
            }
        }
    }

    public void RemoveLowStockIngredient(int ingredientIdx)
    {
        if (lowStockIndices.ContainsKey(ingredientIdx))
        {
            int slotIdx = lowStockIndices[ingredientIdx];

            // Just disable the image, keep GameObject active
            if (slotImages[slotIdx] != null)
            {
                slotImages[slotIdx].enabled = false;
            }

            lowStockIndices.Remove(ingredientIdx);

            ReOrganizeSlots();
        }
    }

    private int FetchSlotIndex()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            // CHANGED LOGIC: Check if the IMAGE is disabled
            if (slotImages[i] != null && !slotImages[i].enabled)
            {
                return i;
            }
        }
        return -1;
    }

    private void ReOrganizeSlots()
    {
        List<int> ingredientIdxs = new(lowStockIndices.Keys);

        lowStockIndices.Clear();

        // 1. Fill up the slots from the start
        for (int i = 0; i < ingredientIdxs.Count; i++)
        {
            int ingredientIdx = ingredientIdxs[i];
            lowStockIndices.Add(ingredientIdx, i);
            IngredientData ingredientData = ingredientDataDict[ingredientIdx];

            if (slotImages[i] != null)
            {
                slotImages[i].sprite = ingredientData.image;

                var col = slotImages[i].color;
                col.a = 1f;
                slotImages[i].color = col;

                slotImages[i].enabled = true;
            }
        }

        // 2. Disable the remaining images (but keep GameObjects active for Grid)
        for (int j = ingredientIdxs.Count; j < slotImages.Length; j++)
        {
            if (slotImages[j] != null)
            {
                slotImages[j].enabled = false;
            }
        }
    }
}