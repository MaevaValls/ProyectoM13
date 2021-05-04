using Atonement.Notifications;

public class Validator {
	public bool isValid { get; private set; }

	public Validator () {
		isValid = true;
	}

	public void Invalidate () {
		isValid = false;
	}
}

public static class ValidatorExtensions {
	//Devuelve una notificación que dice si algo es válido o no
	public static bool Validate (this object target) {
		var validator = new Validator ();
		var notificationName = Global.ValidateNotification(target.GetType()); 
		target.PostNotification (notificationName, validator);
		return validator.isValid;
	}
}