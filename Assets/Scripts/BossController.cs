using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    //references to fill
    [SerializeField] Rigidbody _rb;

    [SerializeField] float _movementSpeed;
    [SerializeField] float _rotationSpeed;

    [SerializeField] GameObject _rotor;
    [SerializeField] GameObject _tailRotor;

    [SerializeField] float _flyingRotorSpeed;
    [SerializeField] float _standbyRotorSpeed;

    [SerializeField] Transform _landingSpot;
    [SerializeField] Transform _takeoffSpot;

    [SerializeField] Transform _northEast_Low;
    [SerializeField] Transform _northWest_Low;
    [SerializeField] Transform _southEast_Low;
    [SerializeField] Transform _southWest_Low;

    private bool isFlying = false;
    private float _rotorSpeed;
    private int phase = 0;
    private Vector3 _targetLocation;
    private int pointsReached;

    //Phase 0- landed/standby
    //Phase 1- moving around outside of arena firing into it
    //Phase 2- dropping bombs from above the arena
    //Phase 3- spinning around, moving around arena shooting


    private void Start()
    {
        //initialize starting values
        isFlying = false;
        _rotorSpeed = _standbyRotorSpeed;
        _targetLocation = _takeoffSpot.position;
        phase = 1;
        pointsReached = 0;

        /*
        //initialize values in the directions dictionary
        directions.Add('n', 0f);
        directions.Add('e', 90f);
        directions.Add('s', 180f);
        directions.Add('w', -90f);

        //initialize values in the movementPoints dictionary
        movementPoints.Add("ne_low", _northEast_Low.position);
        movementPoints.Add("nw_low", _northWest_Low.position);
        movementPoints.Add("se_low", _southEast_Low.position);
        movementPoints.Add("sw_low", _southWest_Low.position);
        */

        Takeoff();
    }

    private void FixedUpdate()
    {
        RotorSpin();
        if (isFlying)
        {
            MoveToTarget();
        }
    }
    //Takeoff function that begins spinning _rotor and _tailRotor and increases altitude
    private void Takeoff()
    {
        isFlying = true;
        _rotorSpeed = _flyingRotorSpeed;
        _targetLocation = _takeoffSpot.position;
    }

    //Land function that decreases altitude and stops _rotor and _tailRotor from spinning
    private void Land()
    {
        _targetLocation = _landingSpot.position;
        _rotorSpeed = _standbyRotorSpeed;
        pointsReached = 0;
    }

    //GeneratePoint function that returns a useable point based on given requirements
    private void GeneratePoint()
    {
        float randomNum = Random.value;
        //if the boss is landed and should now be moving, takeoff
        if (phase != 0 && !isFlying)
        {
            Takeoff();
        } 
        //else, if phase 1 (moving around edge of stage)
        else if(phase == 1)
        {
            //if the boss hasn't been out for too long
            if (pointsReached <= 8)
            {
                //based on the current location, assign a random different target
                //please forgive me for the following wall of if statements
                if (_targetLocation == _northEast_Low.position)
                {
                    if (randomNum >= 0.5)
                    {
                        _targetLocation = _northWest_Low.position;
                    }
                    else
                    {
                        _targetLocation = _southEast_Low.position;
                    }
                }
                else if (_targetLocation == _northWest_Low.position)
                {
                    if (randomNum >= 0.5)
                    {
                        _targetLocation = _northEast_Low.position;
                    }
                    else
                    {
                        _targetLocation = _southWest_Low.position;
                    }
                }
                else if (_targetLocation == _southEast_Low.position)
                {
                    if (randomNum >= 0.5)
                    {
                        _targetLocation = _northEast_Low.position;
                    }
                    else
                    {
                        _targetLocation = _southWest_Low.position;
                    }
                }
                else if (_targetLocation == _southWest_Low.position)
                {
                    if (randomNum >= 0.5)
                    {
                        _targetLocation = _northWest_Low.position;
                    }
                    else
                    {
                        _targetLocation = _southEast_Low.position;
                    }
                }
                else if (_targetLocation == _landingSpot.position)
                {
                    _targetLocation = _takeoffSpot.position;
                }
                else
                {
                    //if the boss is at _takeoffSpot
                    if (randomNum < 0.25)
                    {
                        _targetLocation = _northEast_Low.position;
                    }
                    else if(randomNum <= 0.5)
                    {
                        _targetLocation = _northWest_Low.position;
                    }
                    else if (randomNum < 0.75)
                    {
                        _targetLocation = _southWest_Low.position;
                    }
                    else
                    {
                        _targetLocation = _southEast_Low.position;
                    }
                }
            } //else, if the boss has been out for too long
            else
            {
                if (_targetLocation == _takeoffSpot.position)
                {
                    Land();
                }
                else
                {
                    _targetLocation = _takeoffSpot.position;
                }
            }
        }
    }// end GeneratePoint

    //RotorSpin function that rotates the _rotor and _tailRotor steadily
    private void RotorSpin()
    {
        if (phase != 3)
        {
            _rotor.transform.Rotate(0, _rotorSpeed, 0f, Space.Self);
            _tailRotor.transform.Rotate(0, _rotorSpeed * 2, 0f, Space.Self);
        }
        else
        {
            _rotor.transform.Rotate(0, _rotorSpeed, 0f, Space.Self);
        }
        
    }

    private void MoveToTarget()
    {
        //move the boss' rigidbody towards the target point
        _rb.position = Vector3.MoveTowards(_rb.position, _targetLocation, _movementSpeed);
        //update if the boss has reached the targeted point
        if(_rb.position == _targetLocation)
        {
            if (_rb.position == _landingSpot.position)
            {
                //isFlying = false;
                GeneratePoint();
            }
            else
            {
                pointsReached++;
                GeneratePoint();
            }
        }
    }

    //Dictionary that contains Euler value for cardinal directions
    Dictionary<char, float> directions = new Dictionary<char, float>();

    //Dictionary that contains Vector3 values associated with key points
    Dictionary<string, Vector3> movementPoints = new Dictionary<string, Vector3>();
}