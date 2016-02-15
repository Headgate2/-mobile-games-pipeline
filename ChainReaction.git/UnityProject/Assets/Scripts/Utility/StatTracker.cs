using UnityEngine;
using System.Collections;
using System;

[UnitySingleton(UnitySingletonAttribute.Type.CreateOnNewGameObject, false)]
public class StatTracker : UnitySingleton<StatTracker>
{
	/// <summary>
	/// What time did the application start?
	/// </summary>
	public DateTime StartTime { get; private set; }

	/// <summary>
	/// How long has the application been running for?
	/// </summary>
	public TimeSpan SessionLength { get { return DateTime.Now - StartTime; } }

	/// <summary>
	/// Gets the framerate of the game on this frame
	/// </summary>
	public float CurrentFramesPerSecond { get { return 1f / Time.deltaTime; } }

	//Values used to calculate the average framerate over so much time
	[SerializeField]
	float timeBetweenFpsCount = 0.25f;
	float totalDeltaTime = 0f;
	uint numFrames = 0;
	public float FramesPerSecond { get; private set; }

	public float MaxFPS { get; private set; }
	public float MinFPS { get; private set; }

	protected override void Awake()
	{
		base.Awake();

		StartTime = DateTime.Now;

		MaxFPS = float.MinValue;
		MinFPS = float.MaxValue;
		this.InvokeRepeatForeverSeconds(delegate () {
			FramesPerSecond = numFrames / totalDeltaTime;
			numFrames = 0;
			totalDeltaTime = 0f;
			MaxFPS = Mathf.Max(FramesPerSecond, MaxFPS);
			MinFPS = Mathf.Min(FramesPerSecond, MinFPS);
        }, timeBetweenFpsCount);
	}

	void Update()
	{
		totalDeltaTime += Time.deltaTime;
		numFrames++;
	}
}
