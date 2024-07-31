public class PlayerStateMove : BaseState
{
    public PlayerStateMove(PlayerController playerController) : base(playerController)
    {
    }

    public override void Enter()
    {

    }

    public override void Excute()
    {
        playerController.HandlePlayerMove();
    }

    public override void Exit()
    {

    }

    public override string TypeState()
    {
        return "PlayerRun";
    }
}
