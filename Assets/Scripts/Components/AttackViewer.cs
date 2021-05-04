using System.Collections;
using UnityEngine;
using Atonement.Notifications;
using Atonement.AspectContainer;
using Atonement.Animation;

public class AttackViewer : MonoBehaviour {

	void OnEnable () {
		this.AddObserver (OnValidateAttack, Global.ValidateNotification<AttackAction> ());
	}

	void OnDisable () {
		this.RemoveObserver (OnValidateAttack, Global.ValidateNotification<AttackAction> ());
	}

	void OnValidateAttack (object sender, object args) {
		var action = sender as AttackAction;
		action.perform.viewer = OnPerformAttack;
	}

	//Comportamiento al atacar
	IEnumerator OnPerformAttack (IContainer game, GameAction action) {
		var attackAction = action as AttackAction;
		var board = GetComponent<BoardView> ();
		var attacker = board.GetMatch (attackAction.attacker);
		var target = board.GetMatch (attackAction.target);
		//Si el objeto atacante o recibiendo el ataque no existen cancela la accion
		if (attacker == null || target == null)
			yield break;


		var startPos = attacker.transform.position; 
		var targetStartPos = target.transform.position;
		var toTarget = target.transform.position - startPos;

		//El atacante se mueve hacia el objetivo moviendose en direccion contraria levemente y despues cogiendo velocidad hasta el enemigo (EaseInBack)
		var tweener = attacker.transform.MoveTo (target.transform.position + new Vector3 (0, (float)0.3, 0), 0.5f, EasingEquations.EaseInBack);
		while (tweener != null)
			yield return false;

		//El objetivo retrocede en dirección contraria al atacante.
		var tweener2 = target.transform.MoveTo(target.transform.position + new Vector3((float)(toTarget.x * 0.2), 0, (float)(toTarget.z * 0.2)), 0.3f, EasingEquations.EaseOutElastic);
		while (tweener2 != null)
			yield return false;


		//Aplica el daño
		yield return true;

		//Devolver el atacante y al objetivo a su posicion inicial suavizando la animación al llegar a esta (EaseOutQuad)
		var bounceBack = (toTarget.normalized * (toTarget.magnitude - 0.5f)) + startPos;
		tweener = attacker.transform.MoveTo (bounceBack, 0.2f, EasingEquations.EaseOutQuad);
		while (tweener != null)
			yield return false;

		tweener = attacker.transform.MoveTo (startPos, 0.25f);
		while (tweener != null)
			yield return false;

		tweener = target.transform.MoveTo(targetStartPos, 0.25f);
		while (tweener != null)
			yield return false;
	}
}
