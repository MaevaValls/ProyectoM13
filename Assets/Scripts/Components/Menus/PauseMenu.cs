using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    //ui del menu de pausa
    public GameObject PauseMenuUI;


    //Update se llama a cada frame
    void Update()
    {
        //Si se presiona ESC se actúa segun si el juego está pausado o no
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause(); 
            }
        }    
    }

    public void Resume()
    {
        //Se cierra el menú de pausa y el tiempo transcurre normalmente
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    
    public void Pause()
    {
        //Abre el menú de pasa y hace que el tiempo deje de transcurrir
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        //Carga el menú principal y el tiempo transcurre normalmente
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        //Cierra la aplicación
        Debug.Log("Quiting game...");
        Application.Quit();
    }
}
