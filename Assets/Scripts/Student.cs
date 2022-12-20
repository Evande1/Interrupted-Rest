using UnityEngine;

public class Student : MonoBehaviour
{
	#region Serializables

	[Header("Student Configuration")]

	[SerializeField]
	private Animator animator;
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private string sleepingBoolName = "isSleeping";
	[SerializeField]
	private bool isSleeping = false;

	[SerializeField]
	public float sleepChancePerSecond = 0.01f;

	[Header("Events")]

	[SerializeField, Tooltip("On student getting hit event.")]
	public UnityStudentEvent OnHitEvent = new UnityStudentEvent();

	[SerializeField, Tooltip("On student changing its sleep status event. Keep in mind this will invoke right before the OnHitEvent.")]
	public UnityStudentEvent OnSleepChangeEvent = new UnityStudentEvent();

	[SerializeField, Tooltip("On student wanting to sleep event (the student seeks approval from the level manager to sleep).")]
	public UnityStudentEvent OnWantToSleepEvent = new UnityStudentEvent();

	#endregion

	#region Member Declarations

	/// <summary>
	/// Whether this student is sleeping. Setting the value will make the student sleep/awake.
	/// </summary>
	public bool IsSleeping
	{
		get => isSleeping;
		set
		{
			isSleeping = value;
			animator.SetBool(sleepingBoolName, value);
			OnSleepChangeEvent.Invoke(this);
		}
	}

	private float sleepCheckCooldown = 0.0f;

	#endregion

	#region Monobehaviour

	void Update()
	{
		if (IsSleeping) return;
		if (sleepCheckCooldown <= 0)
		{
			sleepCheckCooldown = 1;
			bool wantToSleep = Random.Range(0.0f, 1.0f) <= sleepChancePerSecond ? true : false;
			if (wantToSleep)
				OnWantToSleepEvent.Invoke(this);

			return;
		}
		sleepCheckCooldown -= Time.deltaTime;
	}
	
	#endregion

	#region Student Functions

	/// <summary>
	/// Student gets bonked by the teacher. Called by the teacher script on their collision event.
	/// </summary>
	public void GetBonked()
	{
		IsSleeping = false;
		OnHitEvent.Invoke(this);
		Debug.Log($"[{name}] got bonked!");
	}

	#endregion

	#region Collision Events

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Collided");
		if (collision.attachedRigidbody.TryGetComponent<PlayerControls>(out _))
		{
			GetBonked();
		}
	}

	#endregion
}
