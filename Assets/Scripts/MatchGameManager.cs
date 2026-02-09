using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class MatchGameManager : MonoBehaviour
{
    [Header("Cài đặt Game")]
    public LineRenderer linePrefab;   
    public float timeLimit = 30f;     
    public int totalPairs = 3;        

    [Header("Giao diện UI")]
    public TMP_Text timerText;        

    // Biến nội bộ
    private LineRenderer currentLine;
    private ConnectNode startNode;
    private int matchCount = 0;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        // 1. TÍNH GIỜ
        timeLimit -= Time.deltaTime;
        if (timerText != null) timerText.text = Mathf.Ceil(timeLimit).ToString(); 

        if (timeLimit <= 0) GameOver(false); 

        // 2. XỬ LÝ CHUỘT
        if (Input.GetMouseButtonDown(0)) StartConnect();
        if (currentLine != null) Drawing();
        if (Input.GetMouseButtonUp(0)) EndConnect();
    }

    void StartConnect()
    {
        // CÁCH MỚI: Dùng Ray từ Camera chiếu thẳng vào thế giới 2D
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        
        // Kiểm tra va chạm
        if (hit.collider != null && hit.collider.CompareTag("Anchor"))
        {
            startNode = hit.collider.GetComponent<ConnectNode>();
            
            // Kiểm tra xem đã nối chưa
            if (startNode.isMatched) return;

            Debug.Log("Đã bấm trúng: " + hit.collider.gameObject.name); // Kiểm tra xem có nhận không

            // Tạo dây
            currentLine = Instantiate(linePrefab, transform);
            currentLine.positionCount = 2;
            currentLine.SetPosition(0, startNode.transform.position);
            currentLine.SetPosition(1, startNode.transform.position);
        }
        else
        {
             // Nếu bấm trượt, log ra để biết
             Debug.Log("Bấm trượt rồi! Hãy kiểm tra Collider."); 
        }
    }

    void Drawing()
    {
        // Cập nhật đầu dây theo chuột
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        currentLine.SetPosition(1, mousePos);
    }

    void EndConnect()
    {
        if (currentLine == null) return;

        // CÁCH MỚI: Kiểm tra điểm thả chuột
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        
        if (hit.collider != null && hit.collider.CompareTag("Anchor"))
        {
            ConnectNode endNode = hit.collider.GetComponent<ConnectNode>();

            if (endNode != null && endNode != startNode &&
                endNode.isLeftColumn != startNode.isLeftColumn && 
                endNode.mathValue == startNode.mathValue &&
                !endNode.isMatched)
            {
                // ==> NỐI ĐÚNG
                currentLine.SetPosition(1, endNode.transform.position);
                startNode.isMatched = true;
                endNode.isMatched = true;
                
                // Đổi màu dây thành xanh lá
                currentLine.startColor = Color.green;
                currentLine.endColor = Color.green;

                currentLine = null; 
                matchCount++;      
                CheckWinCondition();
                Debug.Log("Nối thành công!");
                return;
            }
        }

        // ==> NỐI SAI
        Destroy(currentLine.gameObject); 
        currentLine = null;
        startNode = null;
        Debug.Log("Nối sai hoặc thả ra ngoài!");
    }

    void CheckWinCondition()
    {
        if (matchCount >= totalPairs) GameOver(true);
    }

    void GameOver(bool isWin)
    {
        isGameOver = true;
        if (isWin) LoadingController.LoadScene("Result"); 
        else LoadingController.LoadScene("Result 1");
    }
}