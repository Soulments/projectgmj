using CharacterController;
using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
    }

    public override void OnUpdateState()
    {
        controller.LookAround();
        controller.HandleInventoryInput();
        controller.HandleEscInput();
        controller.HandleItemPickupInput();
        controller.ProcessJumpAttack();
        controller.ActionCheck();
        controller.CheckDeath();
    }

    public override void OnFixedUpdateState()
    {
        controller.SyncBodyPosition();
    }

    public override void OnExitState()
    {
    }
}
