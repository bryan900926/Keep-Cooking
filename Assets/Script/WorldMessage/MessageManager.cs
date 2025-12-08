using UnityEngine;

public class MessageSpawner : MonoBehaviour
{
    public static MessageSpawner Instance;

    [SerializeField] private WorldMessage messagePrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnMessage(string msg, Vector3 worldPos, MessageFlip flip)
    {
        var msgObj = Instantiate(messagePrefab, worldPos, Quaternion.identity);
        msgObj.Show(msg, worldPos, flip);
    }
}

