using UnityEngine;

namespace Accident.Garbage
{

    public class PlayerDetector : MonoBehaviour
    {

    const string PLAYER_TAG = "Player";

        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.CompareTag(PLAYER_TAG) && other.TryGetComponent<Holding>(out Holding holding))
            {
                holding.RemoveLeftoverItems();
            }
        }
    }
}
