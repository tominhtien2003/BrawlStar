public class PlayerStateIdle : BaseState
{
    public PlayerStateIdle(PlayerController playerController) : base(playerController)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Excute()
    {
        playerController.HandlePlayerIdle();
    }

    public override void Exit()
    {
        
    }

    public override string TypeState()
    {
        return "PlayerIdle";
    }
}
