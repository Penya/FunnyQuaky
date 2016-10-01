using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{

	public delegate void TimerAction (float time);

	public event TimerAction OnTimerComplete;
	public event TimerAction OnTimerUpdate;
	public event TimerAction OnTimerPause;

	private float _totalTime;
	private float _currentTime;
	private bool _isPaused;
	private Coroutine _updateTime;

	private float _startTime;

	public float CurrentTime {
		get {
			return this._currentTime;
		}
		set {
			this._currentTime = value;
		}
	}

	public void SetUpTimer (float time)
	{
		this._totalTime = time;
	}

	public void  StartTimer ()
	{
		if (!System.Object.ReferenceEquals (this._updateTime, null)) {
			StopCoroutine (this._updateTime);
			this._updateTime = null;
		}

		this._startTime = Time.time;
		this._currentTime = 0;
		this._isPaused = false;
		this._updateTime = StartCoroutine (UpdateTime ());
	}

	public void PauseTimer ()
	{
		if (!System.Object.ReferenceEquals (this._updateTime, null)) {
			StopCoroutine (this._updateTime);
			this._updateTime = null;
			this._isPaused = true;

			if (!System.Object.ReferenceEquals (OnTimerPause, null)) {
				OnTimerPause (this._currentTime);
			}
		}
	}

	public void ResumeTimer ()
	{
		if (this._isPaused) {
			this._updateTime = StartCoroutine (UpdateTime ());
			this._isPaused = false;
		}
	}

	IEnumerator UpdateTime ()
	{

		while (this._currentTime < this._totalTime) {
			yield return new WaitForFixedUpdate ();
			//this._currentTime = Time.time - this._startTime;
			this._currentTime += Time.fixedDeltaTime;

			if (!System.Object.ReferenceEquals (OnTimerUpdate, null)) {
				OnTimerUpdate (this._currentTime);
			}

		}

		if (!System.Object.ReferenceEquals (OnTimerComplete, null)) {
			StopCoroutine (this._updateTime);
			this._updateTime = null;
			OnTimerComplete (this._currentTime);
		}
	}
}