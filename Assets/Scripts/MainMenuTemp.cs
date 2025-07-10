using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuTemp : MonoBehaviour
{
    public void OnClickPlay()
    {
        Debug.Log("Play clicked.");
        SceneManager.LoadScene(1); // Load scene trực tiếp, tạm bỏ LoadingScreenManager
    }
}
