using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public enum State
    {
        Idle,
        Follow,
        Die,
    }

    public State state;

    public Transform target;
    public float moveSpeed = 3.0f;
    public float rotateSpeed = 3.0f;

    public float followRange = 10.0f;
    public float idleRange = 10.0f;
    public NavMeshAgent agent;
    public Animator animator;
    [HideInInspector] public Collider[] ragdollColliders;
    [HideInInspector] public Rigidbody[] ragdollRigidbodies;

    public bool isDead = false;

    public float GetDistance()
    {
        return (transform.position - target.transform.position).magnitude;
    }

    private void RotateTowardsTarget()
    {
        if (isDead) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotateSpeed * Time.deltaTime);
    }

    private void GoToNextState()
    {
        if (isDead) return;
        string methodName = state.ToString() + "State";

        System.Reflection.MethodInfo info = GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    void Start()
    {
        GoToNextState();
        ragdollColliders = GetComponentsInChildren<BoxCollider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (var collider in ragdollColliders)
        {
            collider.enabled = false;
        }

        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    IEnumerator IdleState()
    {
        Debug.Log("Idle: Enter");
        while (state == State.Idle)
        {
            if (isDead) yield break;
            if(GetDistance() > followRange)
            {
                state = State.Follow;
            }

            yield return 0;
        }
        Debug.Log("idle: Exit");
        GoToNextState();
    }

    IEnumerator FollowState()
    {
        Debug.Log("Follow: Enter");
        while(state == State.Follow)
        {
            if (isDead) yield break;
            agent.destination = target.position;
            RotateTowardsTarget();

            if (GetDistance() < idleRange)
            {
                state = State.Idle;
                agent.destination = transform.position;

            }

            yield return 0;
        }

        Debug.Log("Follow:Exit");
        GoToNextState ();
    }
    IEnumerator DieState()
    {
        Debug.Log("Die: Enter");
        Destroy(this.gameObject);
        yield return 0;
    }

    private void Update()
    {
        if (isDead) return;
        animator.SetFloat("velocity", Vector3.Magnitude(agent.velocity));
    }

}
