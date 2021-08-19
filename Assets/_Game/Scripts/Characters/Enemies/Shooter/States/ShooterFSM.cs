using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class ShooterFSM : StateMachineMB
{
    private Shooter _shooter;

    public ShooterIdleState IdleState { get; private set; }
    public ShooterAggroState AggroState { get; private set; }
    public ShooterHitStunState HitStunState { get; private set; }

    private void Awake()
    {
        _shooter = GetComponent<Shooter>();
        // states
        IdleState = new ShooterIdleState(this, _shooter);
        AggroState = new ShooterAggroState(this, _shooter);
        HitStunState = new ShooterHitStunState(this, _shooter);
    }
}
