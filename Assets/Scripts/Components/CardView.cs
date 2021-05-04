using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour {
	public Image cardBack;
	public Image cardFront;
	public Image cardImage;
	public Text healthText;
	public Text attackText;
	public Text manaText;
	public Text titleText;
	public Text cardText;

	//Booleano para saber si la carta esta boca arriba o abajo y cargar los elementos correspondientes.
	public bool isFaceUp { get; private set; }
	public Card card;
	private GameObject[] faceUpElements;
	private GameObject[] faceDownElements;

	//Asigna los elementos visibles si la carta esta hacia arriba o el reverso si esta boca abajo.
	void Awake () {
		cardImage = gameObject.GetComponentInChildren<Image>();
		cardFront = gameObject.GetComponentInChildren<Image>();
		faceUpElements = new GameObject[] {
			cardImage.gameObject,
			cardFront.gameObject,
			healthText.gameObject, 
			attackText.gameObject, 
			manaText.gameObject, 
			titleText.gameObject,
			cardText.gameObject
	};
		faceDownElements = new GameObject[] {
			cardBack.gameObject
		};
		Flip (isFaceUp);
	}

	//Muestra y esconde los elementos correspondientes a la cara que se ve
	public void Flip (bool shouldShow) {
		isFaceUp = shouldShow;
		var show = shouldShow ? faceUpElements : faceDownElements;
		var hide = shouldShow ? faceDownElements : faceUpElements;
		Toggle (show, true);
		Toggle (hide, false);
		Refresh ();
	}

	//Cambia el estado del booleano al opuesto para los elementos de la vista
	void Toggle (GameObject[] elements, bool isActive) {
		for (int i = 0; i < elements.Length; ++i) {
			elements [i].SetActive (isActive);
		}
	}

	void Refresh () {
		//Si la carta esta boca abajo no hace nada
		if (isFaceUp == false)
			return;

		//Si esta boca arriba carga el coste, nombre y descripcion
		manaText.text = card.cost.ToString ();
		titleText.text = card.name;
		cardText.text = card.text;
		cardImage.sprite = Sprite.Create((Texture2D)Resources.Load(card.image), new Rect(0, 0, 921, 1200), new Vector2(0, 0));

		//Si la carta es un esbirro carga el texto de ataque y vida, si no los vacia
		var minion = card as Minion;
		if (minion != null) {
			attackText.text = minion.attack.ToString ();
			healthText.text = minion.maxHitPoints.ToString ();
		} else {
			attackText.text = string.Empty;
			healthText.text = string.Empty;
		}
	}
}