using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

//TODO finish this and add it to the component menu
//[AddComponentMenu("Event/Event Input Button")]
public class InputEvent : MonoBehaviour
{
	[System.Serializable]
	public struct InputTrigger
	{
		public enum ListenType { Button, Key, MouseButton }
		public enum TriggerType { DownBegin, UpBegin, DownStay, UpStay }
		public enum MouseButton { LeftClick = 0, RightClick = 1, MiddleClick = 2 }

		public ListenType button;
		public TriggerType action;
		
		bool IsButton() { return button == ListenType.Button; }
		bool IsKey() { return button == ListenType.Key; }
		bool IsMouse() { return button == ListenType.MouseButton; }

		[AdvancedInspector.Inspect("IsButton")]
		public string buttonName;

		[AdvancedInspector.Inspect("IsMouse")]
		public MouseButton mouseButton;

		[AdvancedInspector.Inspect("IsKey")]
		public KeyCode key;

		public bool IsActive {
			get {
				if(button == ListenType.Button)
				{
					if (action == TriggerType.DownBegin)
						return Input.GetButtonDown(buttonName);
					else if (action == TriggerType.DownStay)
						return Input.GetButton(buttonName);
					else if (action == TriggerType.UpBegin)
						return Input.GetButtonUp(buttonName);
					else if(action == TriggerType.UpStay)
						return true;
				}
				else if(button == ListenType.Key)
				{
					if (action == TriggerType.DownBegin)
						return Input.GetKeyDown(key);
					else if (action == TriggerType.DownStay)
						return Input.GetKey(key);
					else if (action == TriggerType.UpBegin)
						return Input.GetKeyUp(key);
					else if (action == TriggerType.UpStay)
						return true;
				}
				else if(button == ListenType.MouseButton)
				{
					if (action == TriggerType.DownBegin)
						return Input.GetMouseButtonDown((int)mouseButton);
					else if (action == TriggerType.DownStay)
						return Input.GetMouseButton((int)mouseButton);
					else if (action == TriggerType.UpBegin)
						return Input.GetMouseButtonUp((int)mouseButton);
					else if (action == TriggerType.UpStay)
						return true;
				}
				return false;
			}
		}
	}

	[SerializeField]
	float secondsBetweenCheck;
	public float SecondsBetweenCheck {
		get { return secondsBetweenCheck; }
		set {
			secondsBetweenCheck = value;
		}
	}

	[SerializeField]
	List<InputTrigger> triggers;
	public IEnumerable<InputTrigger> Triggers { get { return triggers; } }

	[SerializeField]
	UnityEvent onActivate;
	public UnityEvent OnActivate { get { return onActivate; } }

	public bool Running { get; private set; }

	//This exists just so the enabled/disabled checkbox appears in the inspector
	void Awake()
	{
		Running = false;
	}

	void OnEnable()
	{
		if (!Running)
			StartCoroutine("InputListener");
	}

	void OnDisable()
	{
		if (Running)
		{
			StopCoroutine("InputListener");
			Running = false;
		}
	}

	IEnumerator InputListener()
	{
		Running = true;
		while (true)
		{
			CheckTrigger();
			if (SecondsBetweenCheck <= 0f)
				yield return null;
			else
				yield return new WaitForSeconds(SecondsBetweenCheck);
		}
	}

	void CheckTrigger()
	{
		foreach(InputTrigger t in triggers)
		{
			if (!t.IsActive)
				return;
		}

		OnActivate.Invoke();
	}
}
