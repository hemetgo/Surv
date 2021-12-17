using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
	float idleTimer;

	public AiStateId GetId()
	{
		return AiStateId.Idle;
	}

	public void Enter(AiAgent agent)
	{
		idleTimer = 0;
		idleTimer = Random.Range(1, 5);
	}

	public void Update(AiAgent agent)
	{
		idleTimer += Time.deltaTime;

		// Patrol
		if (idleTimer >= Random.Range(2, 10))
			agent.stateMachine.ChangeState(AiStateId.Patrol);

		// Chase
		if (agent.PlayerOnView(1))
			agent.stateMachine.ChangeState(AiStateId.Chase);
		
	}

	public void Exit(AiAgent agent)
	{
	}


}
