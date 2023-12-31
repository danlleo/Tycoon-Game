using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitWalkingState : UnitBaseState
{
    public static event EventHandler OnUnitBeganWalking;
    public static event EventHandler OnUnitEndedWalking;

    private Unit _unit;
    private NavMeshAgent _navmeshAgent;

    private float _stoppingDistance = .25f;

    private Vector3 _targetDeskPosition;

    public override void EnterState(UnitStateManager unitStateManager)
    {
        _unit = unitStateManager.GetComponent<Unit>();
        _navmeshAgent = _unit.GetUnitNavmeshAgent();
        _targetDeskPosition = _unit.GetUnitDeskPosition();
        _unit.InvokeUnitMovedEvent();

        OnUnitBeganWalking?.Invoke(_unit, EventArgs.Empty);
    }

    public override void UpdateState(UnitStateManager unitStateManager)
    {
        _navmeshAgent.SetDestination(_targetDeskPosition);
        
        if (Vector3.Distance(_unit.transform.position, _targetDeskPosition) <= _stoppingDistance)
        {
            _navmeshAgent.isStopped = true;
            OnUnitEndedWalking?.Invoke(_unit, EventArgs.Empty);
            unitStateManager.SwitchState(unitStateManager._reachedDeskState);
        }
    }

    public override void ExitState()
    {
        // ...
    }
}
