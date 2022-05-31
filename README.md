# jumper-assignment-vitterb
jumper-assignment-vitterb created by GitHub Classroom
made by: Bert Van Itterbeeck & Jorik van Looke

##1. Setup
  First of all we have to download the right packages, in this case we need ML-Agents. important to note is the version, we use version 2.0.1, because certain things wont work in other versions.
  ![](https://github.com/AP-IT-GH/jumper-assignment-vitterb/blob/1a9ec65f9950cbe34359d4a51ec64f5c12abe232/Pictures/MLl-Agent_Package.JPG)
  
##2. Scene
  The scene is made with only a few components: an agent, a plane and our obstacles. It also has rays, this is to see the obstacles coming. The agent is our AI; it has to jump over the obstacles. it has a lot of scripts, a dedicated Ai script (more later)
  it has a couple of ML-Agent scripts: Decision requester, Behavior parameters, and a rigidbody
  ![](https://github.com/AP-IT-GH/jumper-assignment-vitterb/blob/a80041f48a412e6eb69a08e20d66e4297c390862/Pictures/ML-Agent_scripts.JPG)
  Our obstacle is a prefab of a gameobject which is loaded in randomly and will move over the floor.
  ![](https://github.com/AP-IT-GH/jumper-assignment-vitterb/blob/a80041f48a412e6eb69a08e20d66e4297c390862/Pictures/Obstacle_example.JPG)
##3. Code
###3.1 The Agent
  The first and most important script is the NewBehavior script: this is the AI: At the start of every episode the Agent will be placed on the same coordinates, when the episode starts the AI will look for an obstacle to come. when it sees one with its rays it will jump over the incoming obstacle.
  When the obstacle has passed, the Agent will go through an invisible wall which gives him a reward of 1. if it hits the obstacle it gets a reward of -1. It can also jump to high, if it hits the top gameobject it gets a -0.9.
  
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
   
   ###3.2 the obstacle
    the obstacle has 2 simple scripts: one to move over the scene, and one to let a obstacle spawn at a random time. 
    
   ####3.2.1
    the obstacle script just moves the obstacle over the scene, if it reaches the end of the scene, it will destroy itself.
    
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Obstacle : MonoBehaviour
    {
        public float speed = 3.5f;
    
        private void Update()
        {
            this.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("end"))
            {
                Destroy(this.gameObject);
            }
        }
    }
    
   #### 3.2.2 Spawnscript
   the spawnscript spawns an obstacle object at a random moment, in a range of 1 and 3.5f.
    
    using UnityEngine;

    public class Spawnscript : MonoBehaviour
    {
        public GameObject prefab = null;
        public Transform spawn_point = null;
        public float min = 1.0f;
        public float max = 3.5f;
        // Start is called before the first frame update
        void Start()
        {
            Invoke("Spawn", Random.Range(min, max));
        }

        // Update is called once per frame
        private void Spawn()
        {
            GameObject obstacle = Instantiate(prefab);
            obstacle.transform.position = new Vector3(spawn_point.position.x,spawn_point.position.y,spawn_point.position.z);
            Invoke("Spawn", Random.Range(min, max));
        }
    }

   
   
  
