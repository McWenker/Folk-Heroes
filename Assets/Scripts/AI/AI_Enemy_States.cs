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

    private IAttack attack;
    private IUnit unit;
    private IVision vision;
    private State state;
    [SerializeField] private Transform target;
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
        Debug.Log(state);
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
                target = vision.SearchForFoes();
                if (target == null)
                    state = State.Returning;
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
                        Debug.Log("Attack!");
                        Debug.DrawRay(target.position, Vector3.up, Color.black);
                        unit.ClearMove();
                        state = State.Attacking;
                    });
                }
                break;

            case State.Attacking:
                if(!attack.Attacking && attack.AttackReady)
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
