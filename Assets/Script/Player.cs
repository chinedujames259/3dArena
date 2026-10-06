using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed = 10f;
    [SerializeField] private float JumpHeight = 1f;
    [SerializeField] private float GravityScale = 2f;
    [SerializeField] private float TurnSpeed = 120f;
    private Vector2 MoveInput;
    private bool JumpInput;
    [Header("Components Reference")]
    [SerializeField] CharacterController CharacterController;
    [SerializeField] Animator Animator;
    
    private float VerticalVelocity = 0f;

    #region Input Handling Methods4
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();

    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        
    }
    public void onJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpInput = true;
        }
    }
    #endregion

    #region Unity Callback
    private void Update()
    {
       UpdateMovement();
       UpdateAnimator();
    }

    #endregion

    #region Character control method
    void UpdateMovement()
    {
         Vector3 moveInput3D = new Vector3(MoveInput.x,0f,MoveInput.y);
        Vector3 motion =moveInput3D * Speed* Time.deltaTime;

        UpdatePlayerRotation(moveInput3D);

        if (CharacterController.isGrounded)
        {
            VerticalVelocity = -3f;
        }
        else
        {
            VerticalVelocity += Physics.gravity.y * GravityScale * Time.deltaTime;
        }

        // Jumping
        if (JumpInput && CharacterController.isGrounded)
        {
            VerticalVelocity = Mathf.Sqrt(2f * JumpHeight * Mathf.Abs(Physics.gravity.y * GravityScale));

            JumpInput = false;
        }

        motion.y = VerticalVelocity * Time.deltaTime;

        CharacterController.Move(motion);
    }  
    void UpdatePlayerRotation(Vector3 moveInput)
    {
        if(moveInput.sqrMagnitude < 0.01f)
        {
            return;
        }
        Vector3 PlayerRotation = transform.rotation.eulerAngles;
        PlayerRotation.y = GetAngleFromVector(moveInput);
        //  transform.rotation = Quaternion.Euler(PlayerRotation);

         Quaternion targetRotation = Quaternion.Euler(PlayerRotation);
         float maxDegreesDelta = TurnSpeed * Time.deltaTime;
       transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,maxDegreesDelta);
    }

    float GetAngleFromVector(Vector3 direction)
    {
         Quaternion rotation = Quaternion.LookRotation(direction,Vector3.up);
         return rotation.eulerAngles.y;

    }
    #endregion

    #region Other Method
    void UpdateAnimator()
    {
        bool jump = false;
        bool fall = false;

        if (CharacterController.isGrounded)
        {
            jump = false;
            fall = false;
        }
        else
        {
            if(VerticalVelocity >= 0f)
            {
                jump = true;
            }
            else
            {
                fall = true;
            }
        }


        Vector3 horizontalVelocity = CharacterController.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude;
        Animator.SetFloat("Speed",speed);
        Animator.SetBool("Jump", jump);
        Animator.SetBool("Fall", fall);
    }

    #endregion
}
