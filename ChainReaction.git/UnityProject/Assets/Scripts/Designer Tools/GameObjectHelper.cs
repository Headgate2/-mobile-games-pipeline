using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
	A class that exposes helper functions for manipulating gameobjects, and their transforms.  It also exposes many static Unity functions for editing in the other events.
*/
[AddComponentMenu("Event/Game Object Helper")]
public class GameObjectHelper : MonoBehaviour
{
	///
	/// GameObject options
	///
	public void DestroyGameObject() { Object.Destroy(gameObject); }

	new public void Destroy(Object obj) { Object.Destroy(obj); }

	public void DestroyRootGameObject(Object o)
	{
		GameObject go = MonoExtensions.GetGameObject(o);
		if (go != null)
			Destroy(go.transform.root.gameObject);
	}

	public void DestroyGameObject(Object o) 
	{
		GameObject go = MonoExtensions.GetGameObject(o);
		if(go != null)
			Destroy(go); 
	}
   
	///
	/// Transform Options
	///
	public void SetPostion(Transform t) { transform.position = t.position; }
	public void SetRotation(Transform t) { transform.rotation = t.rotation; }
	public void SetLocalScale(Transform t) { transform.localScale = t.localScale; }

	public void RotateLocalX(float degrees) { RotateLocal(new Vector3(degrees, 0f, 0f)); }
	public void RotateLocalY(float degrees) { RotateLocal(new Vector3(0f, degrees, 0f)); }
	public void RotateLocalZ(float degrees) { RotateLocal(new Vector3(0f, 0f, degrees)); }
	public void RotateLocal(Vector3 degrees) { transform.Rotate(degrees, Space.Self); }

	public void RotateWorldX(float degrees) { RotateWorld(new Vector3(degrees, 0f, 0f)); }
	public void RotateWorldY(float degrees) { RotateWorld(new Vector3(0f, degrees, 0f)); }
	public void RotateWorldZ(float degrees) { RotateWorld(new Vector3(0f, 0f, degrees)); }
	public void RotateWorld(Vector3 degrees) { transform.Rotate(degrees, Space.World); }

	public void MoveLocalX(float move) { MoveLocal(new Vector3(move, 0f, 0f)); }
	public void MoveLocalY(float move) { MoveLocal(new Vector3(0f, move, 0f)); }
	public void MoveLocalZ(float move) { MoveLocal(new Vector3(0f, 0f, move)); }
	public void MoveLocal(Vector3 move) { transform.Translate(move, Space.Self); }

	public void MoveWorldX(float move) { MoveWorld(new Vector3(move, 0f, 0f)); }
	public void MoveWorldY(float move) { MoveWorld(new Vector3(0f, move, 0f)); }
	public void MoveWorldZ(float move) { MoveWorld(new Vector3(0f, 0f, move)); }
	public void MoveWorld(Vector3 move) { transform.Translate(move, Space.World); }

	public void SetRotationLocalX(float rotation) { SetRotationLocal(new Vector3(rotation, 0f, 0f)); }
	public void SetRotationLocalY(float rotation) { SetRotationLocal(new Vector3(0f, rotation, 0f)); }
	public void SetRotationLocalZ(float rotation) { SetRotationLocal(new Vector3(0f, 0f, rotation)); }
	public void SetRotationLocal(Vector3 euler) { transform.localEulerAngles = euler; }
	public void SetRotationLocal(Quaternion rot) { transform.localRotation = rot; }

	public void SetRotationWorldX(float rotation) { SetRotationWorld(new Vector3(rotation, 0f, 0f)); }
	public void SetRotationWorldY(float rotation) { SetRotationWorld(new Vector3(0f, rotation, 0f)); }
	public void SetRotationWorldZ(float rotation) { SetRotationWorld(new Vector3(0f, 0f, rotation)); }
	public void SetRotationWorld(Vector3 euler) { transform.eulerAngles = euler; }
	public void SetRotationWorld(Quaternion rot) { transform.rotation = rot; }

	public void SetPositionLocalX(float position) { SetPositionLocal(new Vector3(position, 0f, 0f)); }
	public void SetPositionLocalY(float position) { SetPositionLocal(new Vector3(0f, position, 0f)); }
	public void SetPositionLocalZ(float position) { SetPositionLocal(new Vector3(0f, 0f, position)); }
	public void SetPositionLocal(Vector3 pos) { transform.localPosition = pos; }

	public void SetPositionWorldX(float position) { transform.position = new Vector3(position, 0f, 0f); }
	public void SetPositionWorldY(float position) { transform.position = new Vector3(0f, position, 0f); }
	public void SetPositionWorldZ(float position) { transform.position = new Vector3(0f, 0f, position); }
	public void SetPositionWorld(Vector3 pos) { transform.position = pos; }

	public void SetParentWorldPosStays(Transform newParent) { transform.SetParent(newParent, true); }
	public void SetParentLocalPosStays(Transform newParent) { transform.SetParent(newParent, false); }

	public void LookAtAndKeepUp(Transform toLookAt) { transform.LookAt(toLookAt, transform.up); }

	public void DetachChildren() { transform.DetachChildren(); }

	///
	/// Event messages for CatchEvent
	///
	public void SendEventHere(string message) { SendMessage("CatchMessage", message, SendMessageOptions.DontRequireReceiver); }
	public void SendEventHereAndParents(string message) { SendMessageUpwards("CatchMessage", message, SendMessageOptions.DontRequireReceiver); }
	public void SendEventHereAndChildren(string message) { BroadcastMessage("CatchMessage", message, SendMessageOptions.DontRequireReceiver); }

	public void GlobalEventInvoke(string eventName) { Messenger.Broadcast(eventName); }

	///
	/// Generating objects from Resources folder.
	///
	public void CreateFromResources(string resourcesDirectory) 
	{
		GameObject prefab = Resources.Load<GameObject>(resourcesDirectory);
		if(prefab != null)
			Instantiate<GameObject>(prefab);
	}

	public void CreateChildFromResources(string resourcesDirectory)
	{
		GameObject prefab = Resources.Load<GameObject>(resourcesDirectory);
		GameObject go = prefab != null ? Instantiate<GameObject>(prefab) : null;
		if(go != null)
			go.transform.SetParent(transform);
	}

	///
	/// Debug Options
	///
	public void DebugBreak() { Debug.Break(); }
	public void DebugLog(string log) { Debug.Log(log); }
	public void DebugLogWarning(string warning) { Debug.LogWarning(warning); }
	public void DebugLogError(string error) { Debug.LogError(error); }

	///
	/// Cursor Options
	///
	public void CursorVisible(bool visible) { Cursor.visible = visible; }
	public void CursorModeConfined() { Cursor.lockState = CursorLockMode.Confined; }
	public void CursorModeLocked() { Cursor.lockState = CursorLockMode.Locked; }
	public void CursorModeNormal() { Cursor.lockState = CursorLockMode.None; }

	///
	/// Time Options
	///
	public void SetTimeScale(float newTimescale) { Time.timeScale = newTimescale; }

	///
	/// Application Options
	///
	public void ApplicationOpenURL(string url) { Application.OpenURL(url); }
	public void ApplicationQuit() { Application.Quit(); }

	///
	///	UI Options
	///
	public void SetTextToProductName(Text uiText) { uiText.text = Application.productName; }
	public void SetTextToCompanyName(Text uiText) { uiText.text = Application.companyName; }
	public void SetTextToVersion(Text uiText) { uiText.text = Application.version; }
	public void SetTextToUnityVersion(Text uiText) { uiText.text = Application.unityVersion; }
	public void SetTextToFramesPerSecond(Text uiText) { uiText.text = StatTracker.Instance.FramesPerSecond.ToString(); }

	/// 
	///	Level Management Options
	///
	public void LevelLoadForce(string levelName) { LevelLoader.Instance.LoadLevelForce(levelName); }
	public void LevelLoadForce(int levelId) { LevelLoader.Instance.LoadLevelForce(levelId); }
	public void LevelReloadForce() { LevelLoader.Instance.LoadLevelForce(LevelLoader.Instance.CurrentSceneId); }

	public void LevelLoadAsyncAuto(string levelName) { LevelLoader.Instance.LoadLevelAsync(levelName, false); }
	public void LevelLoadAsyncAuto(int levelId) { LevelLoader.Instance.LoadLevelAsync(levelId, false); }
	public void LevelReloadAsyncAuto() { LevelLoader.Instance.LoadLevelAsync(LevelLoader.Instance.CurrentSceneId, false); }

	public void LevelLoadAsyncManual(string levelName) { LevelLoader.Instance.LoadLevelAsync(levelName, true); }
	public void LevelLoadAsyncManual(int levelId) { LevelLoader.Instance.LoadLevelAsync(levelId, true); }
	public void LevelReloadAsyncManual() { LevelLoader.Instance.LoadLevelAsync(LevelLoader.Instance.CurrentSceneId, true); }

	public void LevelManualFinishLoad() { LevelLoader.Instance.ManualFinishLoadLevel(); }
}
