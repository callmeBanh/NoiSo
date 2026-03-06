using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class MatchGameManager : MonoBehaviour
{
    [Header("Quản lý Màn chơi")]
    public GameObject[] levels;      // Kéo các Level_1, Level_2 vào đây
    public int[] pairsPerLevel;      // Điền số cặp tương ứng của mỗi màn (Ví dụ: 3, 4)
    private int currentLevelIndex = 0; // Đang ở màn số mấy (0 là màn 1)

    [Header("Cài đặt Game")]
    public LineRenderer linePrefab;   
    public float timeLimitPerLevel = 30f; // Đổi tên: Thời gian cho MỖI màn    

    [Header("Giao diện UI")]
    public TMP_Text timerText;        
    public GameObject tutorialPopup;  

    // Biến nội bộ
    private float currentTime;
    private LineRenderer currentLine;
    private ConnectNode startNode;
    private int matchCount = 0;
    private bool isGameOver = false;
    private bool isPlaying = false;   

    void Start()
    {
        if (tutorialPopup != null) tutorialPopup.SetActive(true);
        isPlaying = false; 
        
        // Vừa vào game thì tải màn đầu tiên
        LoadLevel(0); 
    }

    public void StartGameAfterTutorial()
    {
        if (tutorialPopup != null) tutorialPopup.SetActive(false);
        isPlaying = true; 
    }

    // --- HÀM MỚI: TẢI MÀN CHƠI ---
    void LoadLevel(int index)
    {
        // 1. Tắt tất cả các màn đi
        foreach (GameObject level in levels)
        {
            if (level != null) level.SetActive(false);
        }

        // 2. Chỉ bật màn hiện tại lên
        if (levels[index] != null) levels[index].SetActive(true);

        // 3. Xóa các sợi dây cũ của màn trước (nếu có)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 4. Reset lại đồng hồ và điểm số
        currentTime = timeLimitPerLevel;
        matchCount = 0;
        currentLevelIndex = index;
        isGameOver = false;

        if (timerText != null) timerText.text = Mathf.Ceil(currentTime).ToString();
    }

    void Update()
    {
        if (isGameOver || !isPlaying) return; 

        currentTime -= Time.deltaTime;
        if (timerText != null) timerText.text = Mathf.Ceil(currentTime).ToString(); 

        if (currentTime <= 0) GameOver(false); 

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

    // --- CẬP NHẬT HÀM KIỂM TRA THẮNG ---
    void CheckWinCondition()
    {
        // Lấy số cặp yêu cầu của màn HIỆN TẠI
        int requiredPairs = pairsPerLevel[currentLevelIndex]; 

        if (matchCount >= requiredPairs)
        {
            NextLevel(); // Chuyển sang màn tiếp theo
        }
    }

    // --- HÀM MỚI: CHUYỂN MÀN ---
    void NextLevel()
    {
        currentLevelIndex++; // Tăng số thứ tự màn lên 1

        // Nếu vẫn còn màn trong danh sách
        if (currentLevelIndex < levels.Length)
        {
            Debug.Log("Chúc mừng! Chuyển sang màn: " + (currentLevelIndex + 1));
            LoadLevel(currentLevelIndex); // Bật màn mới
        }
        else
        {
            // Nếu đã vượt qua toàn bộ các màn -> Chiến thắng game
            Debug.Log("ĐÃ PHÁ ĐẢO TOÀN BỘ GAME!");
            GameOver(true);
        }
    }

    void GameOver(bool isWin)
    {
        isGameOver = true;
        isPlaying = false;
        if (isWin) LoadingController.LoadScene("Result"); 
        else LoadingController.LoadScene("Result 1");
    }
}