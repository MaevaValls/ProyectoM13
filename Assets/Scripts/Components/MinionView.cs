using UnityEngine;

public class MinionView : BattlefieldCardView {
	public Sprite inactiveTaunt;
	public Sprite activeTaunt;
	public SpriteRenderer cardImage;

	public Minion minion { get; private set; }
	public override Card card { get { return minion; } }

	public void Display (Minion minion) {
		this.minion = minion;
		Refresh ();
	}

	//Muestra los atributos del esbirro en los textos, la imagen y el marco correspondiente
	protected override void Refresh () {
		cardImage = gameObject.GetComponentInChildren<SpriteRenderer>();
		if (minion == null)
			return;
		if (minion.GetAspect<Taunt> () == null) {
			avatar.sprite = isActive ? active : inactive;
		} else {
			avatar.sprite = isActive ? activeTaunt : inactiveTaunt;
		}
		attack.text = minion.attack.ToString();
		health.text = minion.hitPoints.ToString();
		cardImage.sprite = Resources.Load<Sprite>(card.image + "_small");
	}
}