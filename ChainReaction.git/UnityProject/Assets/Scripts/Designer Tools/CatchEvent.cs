using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
	Event class for catching Unity Message Events that are sent to the GameObject's attached components.

	Events that are captured here are sent from the GameObjectHelper class
*/
[AddComponentMenu("Event/Catch Event")]
public class CatchEvent : MonoBehaviour 
{
	[SerializeField]
	string message;
	public string Message { get { return message; } set { message = value; } }

	[SerializeField]
	UnityEvent onMessageReceived;
	public UnityEvent OnMessageReceived { get { return onMessageReceived; } }

	public void CatchMessage(string message)
	{
		if (message == Message)
			OnMessageReceived.Invoke();
	}
}