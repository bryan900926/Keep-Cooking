using UnityEngine;

public class CookingSpot : MonoBehaviour
{
    [SerializeField] private Transform spot;

    public Transform GetSpot => spot;
}