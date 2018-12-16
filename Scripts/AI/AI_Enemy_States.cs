using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Enemy_States : MonoBehaviour
{
    private enum State
    {
        Idle,
        SearchingFoe,
        ChasingFoe,
        Attacking,
        Returning,
    }

    private IUnit unit;
    private IAttack attack;
    private State state;
    [SerializeField] private Transform target;
    private Vector3 homePoint;

    private void Awake()
    {
        unit = GetComponent<IUnit>();
        attack = GetComponent<IAttack>();
        state = State.Idle;
    }

    private void Start()
    {
        homePoint = transform.position;
        Debug.Log(homePoint);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                if (target != null)
                    state = State.ChasingFoe;
                else
                {
                    unit.Idling();
                    state = State.SearchingFoe;
                }
                break;

            case State.SearchingFoe:
                attack.SearchForFoes();
                target = attack.GetTarget();
                if (target == null)
                    state = State.Returning;
                else
                    state = State.ChasingFoe;
                break;

            case State.ChasingFoe:
                attack.DistanceCheck();
                target = attack.GetTarget();
                if (target == null)
                    state = State.Returning;
                else
                {
                    unit.MoveTo(target.position, 3f, () =>
                    {
                        state = State.Attacking;
                    });
                }
                break;

            case State.Attacking:
                if(attack.AttackReady())
                {
                    attack.DashAttack(transform.position, target.transform.position, attack.DashDistance(), () =>
                    {
                        attack.PlayAnimationAttack(target.transform.position, () =>
                        {
                            state = State.Idle;
                        });
                    });
                }
                else
                    state = State.ChasingFoe;
                break;

            case State.Returning:
                attack.SearchForFoes();
                target = attack.GetTarget();
                if (target != null)
                    state = State.ChasingFoe;
                else
                {
                    unit.MoveTo(homePoint, 0.5f, () =>
                    {
                        state = State.Idle;
                    });
                }
                break;
        }
    }
}
