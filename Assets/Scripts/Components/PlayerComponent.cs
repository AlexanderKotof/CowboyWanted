using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerComponent : CharacterComponent
{
    public Transform bulletSpawnPoint;

    public NavMeshAgent agent;



    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    private void Update()
    {
        animator.SetBool("IsMoving", agent.velocity != Vector3.zero);
    }
}
