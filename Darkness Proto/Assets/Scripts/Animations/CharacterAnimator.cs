using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour {

    const float smoothTime = 0.1f;
    NavMeshAgent agent;
    Animator animator;

	void Start () {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponentInChildren<Animator>();
	}

	void Update () {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent", speedPercent, smoothTime, Time.deltaTime);
	}
}
