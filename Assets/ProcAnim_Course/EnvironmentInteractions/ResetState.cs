using System.Security.Cryptography;
using UnityEngine;

public class ResetState : EnvironmentInteractionState
{
    float _elapsedTime = 0.0f;
    float _resetDuration = 2.0f;
    float _lerpDuration = 10.0f;

    public ResetState(EnvironmentInteractionContext context, EnvironmentInteractionStateMachine.EEnvironmentInteractionState
        stateKey) : base(context, stateKey)
    {
        Context = context;
    }

    public override void EnterState() 
    {
        _elapsedTime = 0.0f;
        Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        Context.CurrentIntersectingCollider = null;
    }

    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset, Context.ColliderCenterY, _elapsedTime / _lerpDuration);
    }

    public override void ExitState() { }

    public override EnvironmentInteractionStateMachine.EEnvironmentInteractionState GetNextState()
    {
        bool isMoving = Context.Rigidbody.linearVelocity != Vector3.zero;
        if (_elapsedTime >= _resetDuration && isMoving)
        {
            return EnvironmentInteractionStateMachine.EEnvironmentInteractionState.Search;
        }

        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {

    }
    public override void OnTriggerStay(Collider other)
    {

    }

    public override void OnTriggerExit(Collider other)
    {

    }
}
