using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 1000.0f;
    // [SerializeField]
    // private float jumpHeight = 1.0f;
    // [SerializeField]
    // private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
     
    private Vector2 movementInput = Vector2.zero;

    [SerializeField]
    private bool isBonking = false;    

    [SerializeField]
    private Animator animator;
   
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context){
        movementInput = context.ReadValue<Vector2>();
    }
    
    public bool isAttacking() {
        return isBonking;
    }

    public void toggleAttacking() {
        isBonking = !isAttacking();
    }

    public void attack(){
        toggleAttacking();
        animator.SetBool("isAttacking", isBonking);
        // toggleAttacking();
        // animator.SetBool("isAttacking", isBonking);
        
    }

    public void onAttack(InputAction.CallbackContext context) {
        Debug.Log("Hello");
        attack();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(movementInput.x, movementInput.y, 0);
        controller.Move(move * Time.deltaTime * playerSpeed * 50);

        // if (move != Vector3.zero)
        // {
        //     gameObject.transform.forward = move;
        // }

        // Changes the height position of the player..
        // if (Input.GetButtonDown("Jump") && groundedPlayer)
        // {
        //     playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        // }

        // playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
