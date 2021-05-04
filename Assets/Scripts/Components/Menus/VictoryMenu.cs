using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{

    //ui del menu de victoria
    public GameObject VictoryMenuUI;

    private void Update()
    {
        //Se activa cuando el estado del juego victoria está activo
        if (GameOverState.GameOverWin == true)
        {
            VictoryMenuUI.SetActive(true);
        }
    }

    public void MainMenu()
    {
        //Al volver al menú principal se quita el estado de derrota y se desactiva el menú de derrota
        SceneManager.LoadScene("Menu");
        VictoryMenuUI.SetActive(false);
        GameOverState.GameOverWin = false;
    }

    public void PlayAgain()
    {
        //Al volver a jugar se quita el estado de derrota y se desactiva el menú de derrota
        GameOverState.GameOverWin = false;
        SceneManager.LoadScene("Game");
        VictoryMenuUI.SetActive(false);
    }

}
