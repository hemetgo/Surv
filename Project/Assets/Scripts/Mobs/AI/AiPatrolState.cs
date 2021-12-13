using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrolState : AiState
{
	float patrolTimer = 0;

	public AiStateId GetId()
	{
		return AiStateId.Patrol;
	}

	public void Enter(AiAgent agent)
	{
		patrolTimer = 0;
		agent.transform.Rotate(0, Random.Range(45, 270), 0);
		agent.animator.SetBool("Walk Forward", true);
	}
	public void Update(AiAgent agent)
	{
		patrolTimer += Time.deltaTime;

		if (agent.IsFrontHitting())
		{
			if (Toolkit.RandomBool()) agent.transform.Rotate(0, Random.Range(-90, -145), 0);
			else agent.transform.Rotate(0, Random.Range(90, 145), 0);
		}

		// Change to Idle
		if (patrolTimer >= Random.Range(2, 5))
			agent.stateMachine.ChangeState(AiStateId.Idle);
		else
			agent.transform.position += agent.transform.forward * agent.moveSpeed * Time.deltaTime;

		// Change to Chase
		if (agent.PlayerOnView(1))
		{
			agent.stateMachine.ChangeState(AiStateId.Chase);
		}
	}

	public void Exit(AiAgent agent)
	{
		agent.animator.SetBool("Walk Forward", false);
	}


}
