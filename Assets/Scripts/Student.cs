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

	[Header("Events")]

	[SerializeField, Tooltip("On student getting hit event.")]
	public UnityStudentEvent OnHitEvent = new UnityStudentEvent();

	[SerializeField, Tooltip("On student changing its sleep status event. Keep in mind this will invoke right before the OnHitEvent.")]
	public UnityStudentEvent OnSleepChangeEvent = new UnityStudentEvent();

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

	#endregion

	#region Monobehaviour

	void Update()
	{

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
	}

	#endregion
}
