using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour {

    Transform target;
    NavMeshAgent agent;

	void Start () {
        agent = this.GetComponent<NavMeshAgent>();
	}

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
    }

    public void MoveToPoint(Vector3 point) {
        agent.SetDestination(point);
    }

    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius - 0.6f;
        agent.updateRotation = false;
        target = newTarget.interactionTransform;
    }
    public void StopFollow()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;
        target = null;
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
