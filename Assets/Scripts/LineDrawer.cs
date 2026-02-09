using UnityEngine;
using System.Collections.Generic;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer linePrefab; // Kéo một LineRenderer prefab vào đây
    private LineRenderer currentLine;
    private ConnectPoint startNode;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) StartDrawing();
        if (currentLine != null) UpdateDrawing();
        if (Input.GetMouseButtonUp(0)) StopDrawing();
    }

    void StartDrawing()
    {
        // Kiểm tra xem có chạm vào điểm nối không
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
        if (hit.collider != null && hit.collider.GetComponent<ConnectPoint>())
        {
            startNode = hit.collider.GetComponent<ConnectPoint>();
            if (startNode.isMatched) return; // Đã nối rồi thì không kéo nữa

            currentLine = Instantiate(linePrefab, transform);
            currentLine.SetPosition(0, startNode.transform.position);
        }
    }

    void UpdateDrawing()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        currentLine.SetPosition(1, mousePos);
    }

    void StopDrawing()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.GetComponent<ConnectPoint>())
        {
            ConnectPoint endNode = hit.collider.GetComponent<ConnectPoint>();

            // Kiểm tra: Khác cột và cùng ID
            if (endNode.isLeftColumn != startNode.isLeftColumn && endNode.id == startNode.id)
            {
                currentLine.SetPosition(1, endNode.transform.position);
                startNode.isMatched = true;
                endNode.isMatched = true;
                currentLine = null; // Giữ lại đường kẻ
                return;
            }
        }

        Destroy(currentLine.gameObject); // Sai thì xóa dây
        currentLine = null;
    }
}