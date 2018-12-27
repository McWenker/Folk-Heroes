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
                // to be expanded later, maybe add patrol functionality
                if (target != null)
                    state = State.ChasingFoe;
                else
                {
                    unit.Idling();
                    state = State.SearchingFoe;
                }
                break;

            case State.SearchingFoe:
                target = attack.SearchForFoes();
                if (target == null)
                    state = State.Returning;
                else
                    state = State.ChasingFoe;
                break;

            case State.ChasingFoe:
                bool hasTarget = attack.DistanceCheck();
                if (!hasTarget)
                {
                    state = State.Returning;
                    target = null;
                }
                else
                {
                    unit.MoveTo(target.position, 3f, () =>
                    {
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
                target = attack.SearchForFoes();
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
