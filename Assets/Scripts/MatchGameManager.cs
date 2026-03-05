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
    public GameObject tutorialPopup;  // Biến mới: Kéo Panel TutorialPopup vào đây

    // Biến nội bộ
    private LineRenderer currentLine;
    private ConnectNode startNode;
    private int matchCount = 0;
    private bool isGameOver = false;
    private bool isPlaying = false;   // Biến mới: Kiểm tra xem đang chơi hay đang xem hướng dẫn

    void Start()
    {
        // Khi mới vào game, hiện Popup lên và CHƯA cho chơi
        if (tutorialPopup != null)
        {
            tutorialPopup.SetActive(true);
        }
        isPlaying = false; 
    }

    // Hàm mới: Được gọi khi bấm nút "Chơi ngay" trên Popup
    public void StartGameAfterTutorial()
    {
        if (tutorialPopup != null)
        {
            tutorialPopup.SetActive(false); // Ẩn Popup đi
        }
        isPlaying = true; // Bắt đầu tính giờ và cho phép nối dây
    }

    void Update()
    {
        // Nếu game kết thúc HOẶC CHƯA BẮT ĐẦU (đang xem popup) -> Không làm gì cả
        if (isGameOver || !isPlaying) return; 

        // 1. TÍNH GIỜ (Chỉ chạy khi isPlaying = true)
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        
        if (hit.collider != null && hit.collider.CompareTag("Anchor"))
        {
            startNode = hit.collider.GetComponent<ConnectNode>();
            if (startNode.isMatched) return;

            currentLine = Instantiate(linePrefab, transform);
            currentLine.positionCount = 2;
            currentLine.SetPosition(0, startNode.transform.position);
            currentLine.SetPosition(1, startNode.transform.position);
        }
    }

    void Drawing()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        currentLine.SetPosition(1, mousePos);
    }

    void EndConnect()
    {
        if (currentLine == null) return;

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
                currentLine.SetPosition(1, endNode.transform.position);
                startNode.isMatched = true;
                endNode.isMatched = true;
                
                currentLine.startColor = Color.green;
                currentLine.endColor = Color.green;

                currentLine = null; 
                matchCount++;      
                CheckWinCondition();
                return;
            }
        }

        Destroy(currentLine.gameObject); 
        currentLine = null;
        startNode = null;
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