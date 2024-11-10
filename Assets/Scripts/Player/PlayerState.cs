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
}

public class PlayerGroundState : BasePlayerState
{
    public PlayerGroundState(PlayerController playerController) : base(playerController) { }
    
    public override void OnEnter()
    {
        playerController.jumpTimes = 1;
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
        playerController.rb.gravityScale = 2;
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
        if (_upTime <= 0 || !playerController.playerInputAction.Gameplay.Jump.IsPressed()
                         || playerController.HeadCheck())
            changeState(EPlayerState.Air);
    }

    public override void OnExit()
    {
        playerController.rb.gravityScale = 15;
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

        playerController.rb.gravityScale = playerController.rb.linearVelocityY > 0 ? 2 : 15;
    }

    public override void OnExit()
    {
        playerController.rb.gravityScale = 10;
    }
}