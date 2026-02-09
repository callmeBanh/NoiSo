using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        LoadingController.LoadScene("testScene");
    }
    public void BackToMainMenu()
    {
        
        Time.timeScale = 1; 
        
        LoadingController.LoadScene("StartGame");
    }

    public void QuitGame()
    {
  
        Debug.Log("Đã bấm thoát game!");

        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}