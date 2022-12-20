using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
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

    private Rigidbody2D rigidbody;
    private Vector2 movementInput { get; set; } = Vector2.zero;

	#endregion

	#region Monobehaviour

	private void Awake()
	{
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
	}

	void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animAttackState))
		{
            rigidbody.velocity = Vector2.zero;
            return;
        }

        float xScale = transform.localScale.x;
        if (movementInput.x != 0)
            xScale = movementInput.x < 0 ? 1 : -1;
        transform.localScale = new Vector3(xScale, 1, 1);
        rigidbody.velocity = movementInput * playerSpeed;
    }

	#endregion

	#region Player Control Functions

	public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        animator.SetTrigger(animAttackTrigger);
    }

    #endregion
}
