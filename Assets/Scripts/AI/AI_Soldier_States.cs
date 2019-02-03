using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Soldier_States : MonoBehaviour
{
    private enum State
    {
        Idle,
        SearchingFoe,
        ChasingFoe,
        Attacking,
        Returning,
    }

    private IAttack attack;
    private IUnit unit;
    private IVision vision;
    private State state;
    private Transform target;
    [SerializeField] private Transform macroTarget;
    private Vector3 homePoint;

    private void Awake()
    {
        unit = GetComponent<IUnit>();
        attack = GetComponent<IAttack>();
        vision = GetComponent<IVision>();
        state = State.Idle;
    }

    private void Start()
    {
        homePoint = transform.position;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                // to be expanded later, maybe add patrol functionality
                /* if (target != null)
                    state = State.ChasingFoe;
                else if(macroTarget != null)
                {
                    unit.MoveTo(macroTarget.position, 0.2f, null);
                    target = vision.SearchForFoes();
                }
                else
                {
                    state = State.SearchingFoe;
                }*/
                break;

            case State.SearchingFoe:
                target = vision.SearchForFoes();
                if (target == null)
                    state = State.Idle;
                else
                    state = State.ChasingFoe;
                break;

            case State.ChasingFoe:
                target = vision.SearchForFoes();
                bool hasTarget = vision.DistanceCheck();
                if (!hasTarget)
                {
                    state = State.Returning;
                    target = null;
                }
                else if(target != null)
                {
                    unit.MoveTo(target.position, attack.AttackRange, () =>
                    {
                        unit.ClearMove();
                        state = State.Attacking;
                    });
                }
                break;

            case State.Attacking:
                if(!attack.Attacking && attack.AttackReady && target != null)
                {
                    attack.CommenceAttack(target.position, () =>
                    {
                        attack.PlayAnimationAttack(target.transform.position, () =>
                        {
                        });
                    });
                }
                else if(attack.Attacking)
                {
                    attack.Attack(() =>
                    {
                        state = State.Idle;
                    });
                }
                else
                    state = State.ChasingFoe;
                break;

            case State.Returning:
                target = vision.SearchForFoes();
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
