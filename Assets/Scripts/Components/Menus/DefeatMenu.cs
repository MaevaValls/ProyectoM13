using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{

    //ui del menu de derrota
    public GameObject DefeatMenuUI;

    private void Update()
    {
        //Se activa cuando el estado del juego derrota está activo
        if (GameOverState.GameOverLose == true)
        {
            DefeatMenuUI.SetActive(true);
        }
    }

    public void MainMenu()
    {
        //Al volver al menú principal se quita el estado de derrota y se desactiva el menú de derrota
        GameOverState.GameOverLose = false;
        SceneManager.LoadScene("Menu");
        DefeatMenuUI.SetActive(false);
    }

    public void PlayAgain()
    {
        //Al volver a jugar se quita el estado de derrota y se desactiva el menú de derrota
        GameOverState.GameOverLose = false;
        SceneManager.LoadScene("Game");
        DefeatMenuUI.SetActive(false);
    }
}
