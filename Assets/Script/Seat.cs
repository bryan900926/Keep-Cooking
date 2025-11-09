using UnityEngine;
using TMPro;

public class Seat : MonoBehaviour
{
    public int SeatNumber { get; set; }

    [SerializeField] private TextMeshPro seatLabel; // assign in Inspector

    public void SetSeatNumber(int number)
    {
        SeatNumber = number;
        seatLabel.text = number.ToString();
    }
}
