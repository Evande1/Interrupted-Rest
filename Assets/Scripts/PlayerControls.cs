using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerControls : MonoBehaviour
{
	#region Serializables

	[SerializeField]
    private Animator animator;
    [SerializeField]
    private float playerSpeed = 1000.0f;
    [SerializeField]
    private bool isBonking = false;
    [SerializeField]
    private string animAttackTrigger = "isAttacking";
    [SerializeField]
    private string animAttackState = "TeacherBonk";

    #endregion

    #region Member Declarations

    private CharacterController controller;
    private Vector2 movementInput { get; set; } = Vector2.zero;

	#endregion

	#region Monobehaviour

	private void Awake()
	{
        controller = gameObject.GetComponent<CharacterController>();
	}

	void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animAttackState))
            return;
        Vector3 move = new Vector3(movementInput.x, movementInput.y, 0);
        controller.Move(move * Time.deltaTime * playerSpeed);
    }

	#endregion

	#region Player Control Functions

	public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        Debug.Log(movementInput);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        animator.SetTrigger(animAttackTrigger);
    }

    #endregion
}
