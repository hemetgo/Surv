using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChaseState : AiState
{
	public AiStateId GetId()
	{
		return AiStateId.Chase;
	}

	public void Enter(AiAgent agent)
	{
		agent.animator.SetBool("Walk Forward", true);
	}
	public void Update(AiAgent agent)
	{
		agent.transform.LookAt(agent.player);
		agent.transform.eulerAngles = new Vector3(0, agent.transform.eulerAngles.y, 0);

		agent.transform.position = Vector3.MoveTowards(agent.transform.position, agent.player.position, agent.moveSpeed * Time.deltaTime);

		// Jump
		if (agent.IsGrounded() && agent.IsFrontHitting())
		{
			agent.Jump();
		}
		
		// Change to Idle
		//if (!agent.PlayerOnView(agent.chaseRangeModifier))
		//	agent.stateMachine.ChangeState(AiStateId.Idle);
	}

	public void Exit(AiAgent agent)
	{
		agent.animator.SetBool("Walk Forward", false);
	}


}
