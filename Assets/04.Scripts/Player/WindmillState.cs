using CharacterController;
using UnityEngine;

public class WindmillState : BaseState
{
    public WindmillState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        controller.StartWindmill();
    }

    public override void OnUpdateState()
    {
        controller.DisableHitBox();
        controller.LookAround();
        controller.HandleInventoryInput();
        controller.HandleEscInput();
        controller.HandleItemPickupInput();
        controller.ProcessWindmillStop();
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
