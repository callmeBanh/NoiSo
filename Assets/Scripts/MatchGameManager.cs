using UnityEngine;
using System.Collections.Generic;

public class MatchGameManager : MonoBehaviour
{
    public LineRenderer linePrefab; // Prefab sợi dây
    private LineRenderer currentLine;
    private ConnectNode startNode;

    void Update()
    {
        // Hỗ trợ cả chuột và chạm trên Web Mobile
        if (Input.GetMouseButtonDown(0)) StartConnect();
        if (currentLine != null) Drawing();
        if (Input.GetMouseButtonUp(0)) EndConnect();
    }

    void StartConnect()
    {
        // Sử dụng Raycast để tìm điểm bắt đầu
        RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Anchor"))
        {
            startNode = hit.collider.GetComponent<ConnectNode>();
            
            // Tạo sợi dây mới
            currentLine = Instantiate(linePrefab, transform);
            currentLine.SetPosition(0, startNode.transform.position);
        }
    }

    void Drawing()
    {
        // Cập nhật đầu dây theo chuột/tay
        currentLine.SetPosition(1, GetMousePos());
    }

    void EndConnect()
    {
        RaycastHit2D hit = Physics2D.Raycast(GetMousePos(), Vector2.zero);
        
        if (hit.collider != null && hit.collider.CompareTag("Anchor"))
        {
            ConnectNode endNode = hit.collider.GetComponent<ConnectNode>();

            // KIỂM TRA LOGIC: Khác cột và cùng giá trị toán học
            if (endNode.isLeftColumn != startNode.isLeftColumn && endNode.mathValue == startNode.mathValue)
            {
                // Nối đúng: Giữ dây lại
                currentLine.SetPosition(1, endNode.transform.position);
                currentLine = null; 
                CheckWinCondition();
                return;
            }
        }

        // Nếu thả tay ra ngoài hoặc sai: Xóa dây
        Destroy(currentLine.gameObject);
        currentLine = null;
    }

    Vector3 GetMousePos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    void CheckWinCondition() { /* Kiểm tra nếu tất cả các cặp đã được nối */ }
}