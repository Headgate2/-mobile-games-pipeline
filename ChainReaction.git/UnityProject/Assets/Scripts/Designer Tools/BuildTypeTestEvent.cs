using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[AddComponentMenu("Event/Build Type Test Event")]
public class BuildTypeTestEvent : MonoBehaviour
{
	[SerializeField, Tooltip("Called after a test, if we are in a development build of the game")]
	UnityEvent debugEvent;
	public UnityEvent DebugEvent { get { return debugEvent; } }
	[SerializeField, Tooltip("Called after a test, if we are in a release build of the game")]
	UnityEvent releaseEvent;
	public UnityEvent ReleaseEvent { get { return debugEvent; } }

	public void Test()
	{
#if DEVELOPMENT_BUILD
		DebugEvent.Invoke();
#else
		ReleaseEvent.Invoke();
#endif
	}
}
