using CharacterController;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
    }

    public override void OnUpdateState()
    {
        controller.LookAround();
        controller.Jump();
        controller.HandleInventoryInput();
        controller.HandleEscInput();
        controller.HandleItemPickupInput();
        controller.ProcessNormalAttack();
        controller.ProcessSkillInputWithoutWindmill();
        controller.ProcessWindmillStart();
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
