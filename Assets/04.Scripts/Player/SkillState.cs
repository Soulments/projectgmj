using CharacterController;
using UnityEngine;

public class SkillState : BaseState
{
    public SkillState(PlayerController controller) : base(controller) { }

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
        controller.ProcessSkillInputWithoutWindmill();
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
