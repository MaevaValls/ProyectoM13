using UnityEngine;
using Atonement.AspectContainer;

public class GameOverState : BaseState {
    public static bool GameOverWin = false;
    public static bool GameOverLose = false;
    public override void Enter () {
		base.Enter ();
        var match = container.GetMatch();
        //Si el index es el del jugador es victoria, si es enemigo derrota
        if(match.currentPlayerIndex == 0)
        {
            GameOverWin = true;
            Debug.Log("Victory");
        } else
        {
            GameOverLose = true;
            Debug.Log("Defeat");
        }
        container.Destroy();
		Debug.Log ("Game Over");
	}
}