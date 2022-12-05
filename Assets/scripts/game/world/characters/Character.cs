using DG.Tweening;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;

    public Animator Animator;

    private bool _moveToDestination;
    private System.Action DestinationCompleteAction;
    private Vector3 DestinationPosition;

    public void DoUpdateFrame()
    {
        if ( CheckDestinationComplete() == true )
        {
            NavMeshAgent.isStopped = true;
            NavMeshAgent.velocity = Vector3.zero;
            if ( DestinationCompleteAction != null )
            {
                DestinationCompleteAction();
                DestinationCompleteAction = null;
                AnimationStateEndRun();
                AnimationStateWork();
            }
        }
    }

    private bool CheckDestinationComplete()
    {
        if ( _moveToDestination == false )
        {
            return false;
        }
        if ( NavMeshAgent.pathPending == false )
        {
            if ( NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance )
            {
                if ( NavMeshAgent.hasPath == false || NavMeshAgent.velocity.sqrMagnitude == 0f )
                {
                    _moveToDestination = false;
                    return true;
                }
            }
        }
        return false;
    }

    public void SetDestination( System.Action returnMethod, Vector3 destinationPosition )
    {
        AnimationStateStartRun();
        DestinationPosition = destinationPosition;
        NavMeshAgent.destination = destinationPosition;
        NavMeshAgent.isStopped = false;
        if ( returnMethod != null )
        {
            _moveToDestination = true;
            DestinationCompleteAction = returnMethod;
        }
    }

    private void AnimationStateStartRun()
    {
        Animator.SetFloat( "Speed_f", 1f );
    }

    private void AnimationStateEndRun()
    {
        Animator.SetFloat( "Speed_f", 0f );
    }

    private void AnimationStateWork()
    {
        this.transform.DOLookAt( DestinationPosition, 0.35f );
        this.Delay( () =>
        {
            AnimationStateStartWork();
            this.Delay( () =>
            {
                AnimationStateEndWork();
            }, 0.75f );
        }, 0.15f );
    }

    private void AnimationStateStartWork()
    {
        Animator.SetBool( "Crouch_b", true );
    }

    private void AnimationStateEndWork()
    {
        Animator.SetBool( "Crouch_b", false );
    }
}