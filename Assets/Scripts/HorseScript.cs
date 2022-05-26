using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class HorseScript : Agent

{
    private Animator _animator;
    private Collider col;
    private int timer = 0;
    private bool jump = false;
    public Transform[] obstacle;


    public override void OnEpisodeBegin()
    {
        _animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }
    void Update()
    {
        if (_animator != null)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                jump = true;
                _animator.SetTrigger("Jump");
                col.enabled = false;
                Debug.Log("spacebar pressed col disabled");
            }
            if (jump)
            {
                timer++;
                if (timer > 250)
                {
                    col.enabled = true;
                    Debug.Log("col enabled");
                    timer = 0;
                    jump = false;
                }
            }
        }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.y = actions.DiscreteActions[0];

        if (jump == false && controlSignal.y == 1)
        {
            jump = true;
            _animator.SetTrigger("Jump");
            col.enabled = false;
            Debug.Log("col disabled");
        }
        if (jump == true)
        {
            timer++;
            if (timer > 250)
            {
                col.enabled = true;
                Debug.Log("col enabled");
                timer = 0;
                jump = false;
            }
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        foreach (var observation in obstacle) { sensor.AddObservation(observation); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-1.0f);
            Debug.Log("hit");
            EndEpisode();
        }
    }
}
