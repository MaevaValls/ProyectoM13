using UnityEngine;

public class DeckView : MonoBehaviour {
	public Transform topCard;
	AudioSource source;
	[SerializeField] Transform squisher;

    public void Start()
    {
        source = GetComponent<AudioSource>();
    }

    //Reduce el tamaño del modelo 3D que representa el mazo a medida que se vacía
    public void ShowDeckSize (float size) {
		squisher.localScale = Mathf.Approximately (size, 0) ? Vector3.zero : new Vector3 (1, size, 1);
	}

    public void PlayDrawCard ()
    {
        source.Play();
    }
}