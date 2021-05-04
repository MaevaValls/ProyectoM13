using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //Al pulsar en el botón de jugar carga la escena del juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        //Al pulsar en el botón de salir muestra un mensaje en la consola y sale de la applicación
        Debug.Log("Quit!");
        Application.Quit();
    }
}
