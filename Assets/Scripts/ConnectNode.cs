using UnityEngine;

public class ConnectNode : MonoBehaviour
{
    [Header("Cấu hình toán học")]
    public int mathValue; // Ví dụ: 1, 2, 3...
    public bool isLeftColumn; // True nếu ở cột vật thể, False nếu ở cột số

    [HideInInspector]
    public bool isMatched = false; // Đã nối xong chưa
}