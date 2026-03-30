using CharacterController;
using UnityEngine;

public class DieState : BaseState
{
    public DieState(PlayerController controller) : base(controller) { }

    public override void OnEnterState()
    {
        controller.OnDieEnter();
    }

    public override void OnUpdateState()
    {
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnExitState()
    {
    }
}
