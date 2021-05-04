using System.Collections.Generic;
using Atonement.AspectContainer;

//Selector que aplicará la habilidad al objetivo que se seleccione
public class ManualTarget : Aspect, ITargetSelector {
	public List<Card> SelectTargets (IContainer game) {
		var card = (container as Ability).card;
		var target = card.GetAspect<Target> ();
		var result = new List<Card> ();
		result.Add (target.selected);
		return result;
	}

	public void Load(Dictionary<string, object> data) {

	}
}
