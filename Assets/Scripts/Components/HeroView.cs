using UnityEngine.UI;

public class HeroView : BattlefieldCardView {
	public Text armor;
	public Hero hero { get; private set; }
	public override Card card { get { return hero; } }

	public void SetHero (Hero hero) {
		this.hero = hero;
		Refresh ();
	}

	//Muestra los textos con los valores del héroe y cambia el marco según si puede atacar
	protected override void Refresh () {
		if (hero == null)
			return;
		avatar.sprite = isActive ? active : inactive;
		attack.text = hero.attack.ToString ();
		health.text = hero.hitPoints.ToString ();
		armor.text = hero.armor.ToString ();
	}
}