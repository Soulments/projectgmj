using CharacterController;
using UnityEngine;

public class HitState : BaseState
{
    public HitState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        controller.OnHitEnter();
    }

    public override void OnUpdateState()
    {
        controller.ActionCheck();
        controller.CheckDeath();
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnExitState()
    {
    }
}
