using UnityEngine;

public class ConnectPoint : MonoBehaviour
{
    public int id; // Ví dụ: 3 vật thể và số 3 đều có ID là 3
    public bool isLeftColumn; // Để phân biệt cột trái và cột phải
    public bool isMatched = false; // Đã nối đúng chưa
}