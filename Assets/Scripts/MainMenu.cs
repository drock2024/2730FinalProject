using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene("Scenes/IntroScene");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ContinueGame() {
        SceneManager.LoadScene("Scenes/SampleScene");
    }

    public void SubmitReport() {
        SceneManager.LoadScene("Scenes/FinalReport");
    }
}
