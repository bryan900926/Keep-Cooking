using UnityEngine;
public class Customerprop : MonoBehaviour
{
    public string customerName;
    public int patience;      // 耐心
    public int orderSize;     // 點餐量
    public float moveSpeed;   // 移動速度

    private void Start()
    {
        // 你也可以在 Start 做一些初始化邏輯，視需求而定
    }
}
