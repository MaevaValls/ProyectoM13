using System.Collections.Generic;
using Atonement.AspectContainer;

public class Card : Container {
	public string id;
	public string name;
	public string text;
	public string image;
	public int cost;
	public int orderOfPlay = int.MaxValue;
	public int ownerIndex;
	public Zones zone = Zones.Deck;

	//Recibe los datos del JSON
	public virtual void Load (Dictionary<string, object> data) {
		id = (string)data ["id"];
		name = (string)data ["name"];
		text = (string)data ["text"];
		image = (string)data ["image"];
		cost = System.Convert.ToInt32(data["cost"]);
	}
}