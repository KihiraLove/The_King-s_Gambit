using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void PlayButton()
    {
        Debug.Log("Play Clicked");
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void QuitButton()
    {
        Debug.Log("Quit Clicked");
        // Quit Game
        Application.Quit();
    }

    public void ExitButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}