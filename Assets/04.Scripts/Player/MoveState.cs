using CharacterController;
using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
    }

    public override void OnUpdateState()
    {
        controller.LookAround();
        controller.Move();
        controller.Jump();
        controller.HandleInventoryInput();
        controller.HandleEscInput();
        controller.HandleItemPickupInput();
        controller.ProcessNormalAttack();
        controller.ProcessComboAttack();
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
