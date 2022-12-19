using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour
{
	#region Serializables

	[Header("Tooltips")]

	[SerializeField]
	private Animator animator;
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private string sleepingBoolName = "isSleeping";

	private bool isSleeping = false;

	public bool IsSleeping {
		get {
			return isSleeping;
		}
		set {
			isSleeping = value;
			animator.SetBool(sleepingBoolName, value);
		}
	}

	#endregion

	#region Monobehaviour

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		
		Respond();
	}

	void Respond()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            IsSleeping = false;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            IsSleeping = true;
        }
    }



	#endregion

	#region Student Functions

	#endregion
}
