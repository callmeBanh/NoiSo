using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        LoadingController.LoadScene("testScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
