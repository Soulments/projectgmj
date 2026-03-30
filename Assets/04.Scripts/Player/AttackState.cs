using CharacterController;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
    }

    public override void OnUpdateState()
    {
        controller.DisableHitBox();
        controller.DisableAttack();
        controller.LookAround();
        controller.HandleInventoryInput();
        controller.HandleEscInput();
        controller.HandleItemPickupInput();
        controller.ProcessNormalAttack();
        controller.ProcessComboAttack();
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
