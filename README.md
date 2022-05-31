# jumper-assignment-vitterb
made by: Bert Van Itterbeeck & Jorik Van Looke

## 1. Setup
###1.1 Packages
the first thing we have to do is to download the necessary packages. The most important one is the ML-Agents package. this will make you able to train your AI and give you some other functionality.

![](https://github.com/AP-IT-GH/jumper-assignment-vitterb/blob/e947a60ca034f454072c62c7859959a6c8c704ff/Pictures/MLl-Agent_Package.JPG)

We use another package in our project: cinemachine. We use one of the older versions because this is the most stable version.

![](https://github.com/AP-IT-GH/jumper-assignment-vitterb/blob/e947a60ca034f454072c62c7859959a6c8c704ff/Pictures/Cinemachine_Package.JPG)

###Scene 
after downloading all the packages we need, we start by making our scene. We make an agent, which is just a capsule game object. We make a plane which will act as the floor of the scene and last but not least we made our obstacles. For moving our obstacles we made use of the cinemachine, You just make give the floor a script named cinemachine path. Then we give coordinates for all the waypoints. 

![](https://github.com/AP-IT-GH/jumper-assignment-vitterb/blob/e947a60ca034f454072c62c7859959a6c8c704ff/Pictures/Cinemachine_example.JPG)

Now we have our path, we need to make our obstacles move on teh path. We do this by adding the "Cinemachine dolly cart" script to every obstacle. We do also need to give every obstacke a Tag: obstacle.  

![](https://github.com/AP-IT-GH/jumper-assignment-vitterb/blob/e947a60ca034f454072c62c7859959a6c8c704ff/Pictures/Cinemachine_Obstacle_example.JPG)

We also need to setup our agent, we need to give our agent at least 4 Components: a rigidbody if its not already there, the Behavior parameters, Decision requester, and the Ray Perceptor 3D

![](https://github.com/AP-IT-GH/jumper-assignment-vitterb/blob/e947a60ca034f454072c62c7859959a6c8c704ff/Pictures/Agent_Scripts_example.JPG)

## 2. Scripts
There is one script we need to write ourselves: our agentscript to train the AI. First thing in the script is the packages we need for this script. A couple of functionalities from ML-Agents, and some basic unity functionalities. Then we make all the variables we need: a bool isgrouded to see if our agent is already jumping or not. A rigidbody, another bool isObstaclePassed, to see is the obstacle is, well, passed already, and a public float jumpspeed to make you able to change te speed in which the agent jumps. In the Initialize we initialize the rigidbody by searching the gameobject for one. Then we add an observation on to the agent: Now the agent knows where he is himself. Then the Rewward system: when isGrounded is true and the first continouosAction is 1.0f then the Agent will jump and isGrounded wil become false. Then it is a case of knowing is the obstacle has passed, if this is true, the agent gets 1.0f points. if you got your points, isObstaclePassed is set back to true and your episode ends. Last but not leasst you'll find the heuristic function: this is a function where we can test all the functionalities manually to see if everything work before we start the training
  
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
            private bool isObstaclePassed = false;
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
              if (actionBuffers.ContinuousActions[0] == 1.0f)
              {
                if (isGrounded)
                {
                  body.velocity = new Vector3(0, jumpSpeed, 0);
                  Debug.Log("Action");
                  isGrounded = false;
                }
              } 
              if (isObstaclePassed)
              {
                AddReward(1.0f);
                Debug.Log("Passed");
                isObstaclePassed = false;
                EndEpisode();
              }
          }
          
          private void OnCollisionEnter(Collision collision)
          {
            Debug.Log($"Collision with {collision.gameObject.name}");
            if (collision.gameObject.CompareTag("Road"))
            {
              isGrounded = true;
            }

            if (collision.gameObject.CompareTag("Obstacle"))
            {
              SetReward(-1.0f);
              Debug.Log("Failed");
              collision.gameObject.CompareTag("Obstacle");
              EndEpisode();
            }
          }
          
          // Testen van de omgeving -> Behavior Parameters script -> Behavior Type
          public override void Heuristic(in ActionBuffers actionsOut)
          {
            var continuousActionsOut = actionsOut.ContinuousActions;
            if (Input.GetButton("Jump"))
            {
              continuousActionsOut[0] = 1.0f;
            }
          }
        }
