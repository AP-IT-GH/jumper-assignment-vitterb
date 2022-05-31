using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

public class NewBehaviourScript : Agent
{
    public float force = 15f;
    public Transform reset = null;
    private Rigidbody rb = null;
    private bool jump;

    public override void Initialize()
    {
        rb = this.GetComponent<Rigidbody>();
        ResetMyAgent();
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.ContinuousActions[0] > 0)
        {
            if (!jump)
            {
                Thrust();
            }
        }
    }
    public override void OnEpisodeBegin()
    {
        ResetMyAgent();
        jump = false;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle") == true)
        {
            AddReward(-1.0f);
            Destroy(collision.gameObject);
            EndEpisode();
        }
        if (collision.gameObject.CompareTag("top") == true)
        {
            AddReward(-0.9f);
            EndEpisode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("reward") == true)
        {
            AddReward(0.1f);
        }
    }
    private void Thrust()
    {
        rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
    }
    private void ResetMyAgent()
    {
        this.transform.position = new Vector3(reset.position.x, reset.position.y, reset.position.z);
    }

}