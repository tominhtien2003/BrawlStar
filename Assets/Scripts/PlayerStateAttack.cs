using UnityEngine;

public class PlayerStateAttack : BaseState
{
    public PlayerStateAttack(PlayerController playerController) : base(playerController)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Excute()
    {
        playerController.HandlePlayerAttack();
    }

    public override void Exit()
    {
        
    }

    public override string TypeState()
    {
        return "PlayerAttack";
    }
}
