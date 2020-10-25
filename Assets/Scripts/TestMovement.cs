using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        Vector3 test = new Vector3(2f, transform.position.y, 4f);
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.SetDestination(test);
    }
}
