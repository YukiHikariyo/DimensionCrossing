using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D       rb;
    public PlayerInputAction playerInputAction;

    private BasePlayerState _currentState;
    private BasePlayerState _groundState;
    private BasePlayerState _jumpState;
    private BasePlayerState _airState;
    private BasePlayerState _ladderState;
    
    [Space(16)]
    [Header("基础数值")]
    [Space(16)]
    
    public float moveSpeed;
    public float jumpSpeed;
    public float ladderSpeed;
    [Space(16)]
    public float jumpTimes;
    [Space(16)]
    public float jumpGravity;
    public float fallGravity;
    
    [Space(16)]
    [Header("范围检测")]
    [Space(16)]
    
    public LayerMask groundLayer;
    public LayerMask ladderLayer;
    
    public Vector2 groundCheckCenter;
    public Vector2 groundCheckSize;
    
    public Vector2 headCheckCenter;
    public Vector2 headCheckSize;
    
    public Vector2 ladderCheckCenter;
    public Vector2 ladderCheckSize;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInputAction = new PlayerInputAction();

        _groundState = new PlayerGroundState(this);
        _jumpState = new PlayerJumpState(this);
        _airState = new PlayerAirState(this);
        _ladderState = new PlayerLadderState(this);
    }

    private void OnEnable()
    {
        playerInputAction.Gameplay.Jump.started += Jump;
        playerInputAction.Gameplay.VerticalMove.started += ClimbLadder;
        playerInputAction.Enable();
        
        ChangeState(EPlayerState.Ground);
    }
    
    private void OnDisable()
    {
        _currentState.OnExit();
        
        playerInputAction.Disable();
        playerInputAction.Gameplay.Jump.started -= Jump;
        playerInputAction.Gameplay.VerticalMove.started -= ClimbLadder;
    }

    private void FixedUpdate()
    {
        _currentState.PhysicsUpdate();
    }

    private void Update()
    {
        _currentState.LogicUpdate();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + groundCheckCenter, groundCheckSize);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + headCheckCenter, headCheckSize);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + ladderCheckCenter, ladderCheckSize);
    }

    public void ChangeState(EPlayerState newState)
    {
        _currentState?.OnExit();
        _currentState = newState switch
                        {
                            EPlayerState.Ground => _groundState,
                            EPlayerState.Jump   => _jumpState,
                            EPlayerState.Air    => _airState,
                            EPlayerState.Ladder => _ladderState,

                            _ => null
                        };
        _currentState?.OnEnter();
    }
    
    public bool GroundCheck() => Physics2D.OverlapBox((Vector2)transform.position + groundCheckCenter
                                                    , groundCheckSize, 0, groundLayer);
    
    public bool HeadCheck() =>  Physics2D.OverlapBox((Vector2)transform.position + headCheckCenter
                                                    , headCheckSize, 0, groundLayer);
    
    public bool LadderCheck() => Physics2D.OverlapBox((Vector2)transform.position + ladderCheckCenter
                                                    , ladderCheckSize, 0, ladderLayer);

    public void HorizontalMove()
    {
        if (playerInputAction.Gameplay.HorizontalMove.IsPressed())
            rb.linearVelocityX = playerInputAction.Gameplay.HorizontalMove.ReadValue<float>() * moveSpeed;
    }
    
    public void LadderMove()
    {
        rb.linearVelocity = new Vector2(playerInputAction.Gameplay.HorizontalMove.ReadValue<float>() * ladderSpeed
                                      , playerInputAction.Gameplay.VerticalMove.ReadValue<float>() * ladderSpeed);
    }

    public void Jump(InputAction.CallbackContext obj)
    {
        if (jumpTimes <= 0)
            return;

        jumpTimes--;
        ChangeState(EPlayerState.Jump);
    }
    
    private void ClimbLadder(InputAction.CallbackContext obj)
    {
        if (LadderCheck())
            ChangeState(EPlayerState.Ladder);
    }
}