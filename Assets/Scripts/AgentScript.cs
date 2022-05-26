using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentScript : Agent
{
    [SerializeField]
    float AgentJumpSpeed = 10;
    private Rigidbody rb;
    private bool Grounded;
    private int counter = 0;    

    public override void Initialize()
    {
        base.Initialize();
        rb = GetComponent<Rigidbody>();
        Grounded = true;
    }

    private void FixedUpdate()
    {
        if (counter == 10000)
        {
            EndEpisode();
            Debug.Log("endEpisode");
        } 
        counter++;  
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var contActions = actionsOut.ContinuousActions;

        if (Input.GetKey(KeyCode.Space))
        {
            contActions[0] = 1f;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (actions.ContinuousActions[0] == 1)
        {
            if (Grounded)
            {
                rb.velocity = new Vector3(0,AgentJumpSpeed,0);
                Grounded = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
        }
        else if (collision.gameObject.CompareTag("Obstacle")) 
        {
            AddReward(-1.0f);
            Debug.Log("hit");
            EndEpisode();
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // target en Agent posities

        sensor.AddObservation(this.transform.localPosition);
    }
}
