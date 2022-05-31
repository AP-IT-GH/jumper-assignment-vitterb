using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents;

public class jumper_agent : Agent
{
    private bool isGrounded = true;
    private Rigidbody body;
    [SerializeField] public float jumpSpeed = 10.0f;

    public override void Initialize()
    {
        body = GetComponent<Rigidbody>();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        
        
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (actionBuffers.ContinuousActions[0] > 0)
        {
            if (isGrounded)
            {
                body.AddForce(Vector3.up * jumpSpeed, ForceMode.Acceleration);
                Debug.Log("Action");
                isGrounded = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision with {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("field"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("obstacle"))
        {
            SetReward(-1.0f);
            Debug.Log("Failed");
            collision.gameObject.CompareTag("obstacle");
            EndEpisode();
        }
    }

    // Testen van de omgeving -> Behavior Parameters script -> Behavior Type
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.Space))
        {
            continuousActionsOut[0] = 1.0f;
        }
    }

}