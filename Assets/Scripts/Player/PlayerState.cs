using System;
using UnityEngine;

public abstract class BasePlayerState
{
    protected readonly PlayerController     playerController;
    protected readonly Action<EPlayerState> changeState;

    protected BasePlayerState(PlayerController playerController)
    {
        this.playerController = playerController;
        changeState = playerController.ChangeState;
    }

    public abstract void OnEnter();
    public abstract void PhysicsUpdate();
    public abstract void LogicUpdate();
    public abstract void OnExit();
}

public enum EPlayerState
{
    Ground,
    Jump,
    Air,
    Ladder,
}

public class PlayerGroundState : BasePlayerState
{
    public PlayerGroundState(PlayerController playerController) : base(playerController) { }
    
    public override void OnEnter()
    {
        playerController.jumpTimes = 1;
        playerController.rb.gravityScale = playerController.fallGravity;
    }

    public override void PhysicsUpdate()
    {
        playerController.HorizontalMove();
    }

    public override void LogicUpdate()
    {
        if (!playerController.GroundCheck())
            changeState(EPlayerState.Air);
    }

    public override void OnExit()
    {
        
    }
}

public class PlayerJumpState : BasePlayerState
{
    private float _upTime;
    
    public PlayerJumpState(PlayerController playerController) : base(playerController) { }

    public override void OnEnter()
    {
        playerController.rb.gravityScale = playerController.jumpGravity;
        _upTime = 0.25f;
    }

    public override void PhysicsUpdate()
    {
        playerController.HorizontalMove();
        
        if (playerController.playerInputAction.Gameplay.Jump.IsPressed())
            playerController.rb.linearVelocityY = playerController.jumpSpeed;
    }

    public override void LogicUpdate()
    {
        _upTime -= Time.deltaTime;
        if (!playerController.playerInputAction.Gameplay.Jump.IsPressed())
        {
            playerController.rb.linearVelocityY = playerController.jumpSpeed * 0.2f;
            changeState(EPlayerState.Air);
        }
        else if (_upTime <= 0 || playerController.HeadCheck())
            changeState(EPlayerState.Air);
        
    }

    public override void OnExit()
    {
        playerController.rb.gravityScale = playerController.fallGravity;
    }
}

public class PlayerAirState : BasePlayerState
{
    public PlayerAirState(PlayerController playerController) : base(playerController) { }
    
    public override void OnEnter()
    {
        
    }

    public override void PhysicsUpdate()
    {
        playerController.HorizontalMove();
    }

    public override void LogicUpdate()
    {
        if (playerController.GroundCheck())
            changeState(EPlayerState.Ground);

        playerController.rb.gravityScale = playerController.rb.linearVelocityY > 0
                                               ? playerController.jumpGravity 
                                               : playerController.fallGravity;
    }

    public override void OnExit()
    {
        playerController.rb.gravityScale = playerController.fallGravity;
    }
}

public class PlayerLadderState : BasePlayerState
{
    public PlayerLadderState(PlayerController playerController) : base(playerController) { }

    public override void OnEnter()
    {
        playerController.rb.gravityScale = 0;
        playerController.jumpTimes = 1;
    }

    public override void PhysicsUpdate()
    {
        playerController.LadderMove();
    }

    public override void LogicUpdate()
    {
        if (!playerController.LadderCheck())
            changeState(EPlayerState.Ground);
    }

    public override void OnExit()
    {
        playerController.rb.gravityScale = playerController.fallGravity;
    }
}