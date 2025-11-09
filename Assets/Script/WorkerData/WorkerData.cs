using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "worker", menuName = "WorkerData/FrontWorker")]
public class WorkerData : ScriptableObject
{
    public Sprite image;

    public int id;

    public string workType; // 1 = front, 0 = back

}