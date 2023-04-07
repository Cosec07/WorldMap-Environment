using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;
using Unity.MLAgents.Policies;

public class BoatAgent : Agent
{
    //[SerializeField] private Transform targetTransform;
    //[SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    [SerializeField] private GameObject boat;
    //[SerializeField] private GameObject episode;


    private int epNo = 0;
    private int i = 0;
    //private BehaviorType HeuristicOnly;
    private float accuracy = 0;
    [SerializeField] float moveSpeed = 1f;
    private float prevDis;
    private float directionX;
    private float directionZ;
    /*public override void OnEpisodeBegin()
    {
        epNo += 1;
        Debug.Log(epNo + " accuracy: " + accuracy / epNo * 100 + "%");
        //episode.GetComponent<Text>().text = "Episode: " + epNo;
        //pointB.transform.localPosition = new Vector3(Random.Range(-1.6f, 1.3f), 0.41f, Random.Range(2.5f, 4f));
        transform.parent.transform.localPosition = new Vector3(-2f, 0.53f, -4f);
        transform.localPosition = new Vector3(0, 0, 0);
        //transform.Rotate(0, 0, 0);
        //transform.position = pointA.transform.position + new Vector3(0.5f, 0, 0.5f);
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;

    } */
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(targetTransform.localPosition);
        //sensor.AddObservation(pointA.transform.localPosition);
        sensor.AddObservation(pointB.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //int movement = actions.DiscreteActions[0];
        float MoveX = actions.ContinuousActions[0];
        float MoveZ = actions.ContinuousActions[1];
        //Debug.Log(movement);
        //if (movement == 0)
        //{
        //directionX = 0;
        //directionZ = 0;
        //}
        //if (movement == 1) { 
        //directionX = -1;
        //}
        //if (movement == 2) { 
        //directionX = 1;
        //}
        //if (movement == 3) {
        //directionZ = -1;
        //}
        //if (movement == 4) {
        //directionZ = 1;
        //}
        //transform.GetComponent<Rigidbody>().AddForce(new Vector3(directionX * 1f, 0f, directionZ * 1f));


       transform.localPosition += new Vector3(MoveX, 0, MoveZ) * Time.deltaTime * moveSpeed;
        Vector3 movementDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (movementDir != Vector3.zero)
        {
            transform.forward = movementDir;
        }
    }
    //for testing purpose only
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        /*ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.S))
        {
            discreteActions[0] = 3;
        }
        if (Input.GetKey(KeyCode.W))
        {
            discreteActions[0] = 4;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActions[0] = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discreteActions[0] = 1;
        }
        if (Input.GetKey(KeyCode.B))
        {
            discreteActions[0] = 0;
        }*/
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void Update()

    {
       // BehaviorParameters behaviorParameters;
       // behaviorParameters = boat.GetComponent<BehaviorParameters>();
        //behaviorParameters.BehaviorType = BehaviorType.HeuristicOnly;
       // Debug.Log(behaviorParameters.BehaviorType);
        float prevDis = 999f;
        float currDis = Vector3.Distance(boat.transform.localPosition, pointB.transform.localPosition);

        if (prevDis - currDis >= 1)
        {
            AddReward(+0.01f);
        }
        else
        {
            AddReward(-0.02f);
        }
        prevDis = currDis;
    }
    private void OnTriggerEnter(Collider other)
    {

       /* if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-10.0f);
            boat.GetComponent<Renderer>().material.color = Color.red;
            //EndEpisode();
        }
        if (other.TryGetComponent<Whale>(out Whale whale))
        {
            SetReward(-15.0f);
            boat.GetComponent<Renderer>().material.color = Color.magenta;
            //EndEpisode();
        }
        /*        if (other.TryGetComponent<Checkpoint>(out Checkpoint cp))
                {
                    SetReward(+1.0f);
                    boat.GetComponent<Renderer>().material.color = Color.yellow;

                }
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+15f);
            boat.GetComponent<Renderer>().material.color = Color.green;
            EndEpisode();
            accuracy += 1;

        } */



    }
}
