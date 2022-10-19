using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    //references to fill
    [SerializeField] Rigidbody _rb;
    [SerializeField] Health _bossHealth;
    [SerializeField] WeaponBase _wb;
    [SerializeField] WeaponBombs _bombSystem;
    [SerializeField] Slider _bossHealthbar;

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

    [SerializeField] Transform _north_Low;
    [SerializeField] Transform _east_Low;
    [SerializeField] Transform _south_Low;
    [SerializeField] Transform _west_Low;

    [SerializeField] Transform _northEast_High;
    [SerializeField] Transform _northWest_High;
    [SerializeField] Transform _southEast_High;
    [SerializeField] Transform _southWest_High;

    private bool isFlying = false;
    private float _rotorSpeed;
    private int phase = 0;
    private Vector3 _targetLocation;
    private bool _attacking;
    private float _maxMovementSpeed;

    //North is 0, East is 90, South is 180, West is 270
    private float _directionToFace;
    
    private int pointsReached;

    //Phase 0- landed/standby
    //Phase 1- moving around outside of arena firing into it
    //Phase 2- dropping bombs from above the arena
    //Phase 3- spinning around, moving around arena shooting

    //Dictionary that contains Euler value for cardinal directions
    Dictionary<char, float> directions = new Dictionary<char, float>();

    private void OnEnable()
    {
        _bossHealth.DamageTaken += UpdateHealthbar;
    }

    private void OnDisable()
    {
        _bossHealth.DamageTaken -= UpdateHealthbar;
    }

    private void Start()
    {
        //initialize starting values
        isFlying = false;
        _rotorSpeed = _standbyRotorSpeed;
        _targetLocation = _takeoffSpot.position;
        _bossHealthbar.maxValue = _bossHealth.GetHealth();
        _bossHealthbar.value = _bossHealthbar.maxValue;

        phase = 1;

        _directionToFace = transform.rotation.eulerAngles.y;
        pointsReached = 0;
        _maxMovementSpeed = _movementSpeed;

        //initialize values in the directions dictionary
        directions.Add('n', 0f);
        directions.Add('e', 90f);
        directions.Add('s', 180f);
        directions.Add('w', 270f);

        /*
        //initialize values in the movementPoints dictionary
        movementPoints.Add("ne_low", _northEast_Low.position);
        movementPoints.Add("nw_low", _northWest_Low.position);
        movementPoints.Add("se_low", _southEast_Low.position);
        movementPoints.Add("sw_low", _southWest_Low.position);
        */

        StartCoroutine(Takeoff());
    }

    private void Update()
    {
        if(phase == 1 && _attacking)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        RotorSpin();
        if (isFlying)
        {
            MoveToTarget();
            if(phase == 1)
            {
                RotateToFace();
            }
        }
    }
    //Takeoff function that begins spinning _rotor and _tailRotor and increases altitude
    private IEnumerator Takeoff()
    {
        Debug.Log("Taking Off!");
        yield return new WaitForSeconds(3f);
        isFlying = true;
        _rotorSpeed = _flyingRotorSpeed;
        _targetLocation = _takeoffSpot.position;
        _directionToFace = directions['s'];
        if(phase == 2)
        {
            _movementSpeed = _maxMovementSpeed / 2;
        }
        else
        {
            _movementSpeed = _maxMovementSpeed;
        }
    }

    //Land function that decreases altitude and stops _rotor and _tailRotor from spinning
    private void Land()
    {
        _targetLocation = _landingSpot.position;
        _rotorSpeed = _standbyRotorSpeed;
        pointsReached = 0;
        _directionToFace = directions['s'];
        if (Phase3Check())
        {
            phase = 3;
        }
        else
        {
            float randomNum = Random.value;
            if(randomNum < 0.6)
            {
                phase = 1;
            }
            else
            {
                phase = 2;
            }
        }
    }

    //GeneratePoint function that returns a useable point based on given requirements
    private void GeneratePoint()
    {
        float randomNum = Random.value;
        //if the boss is landed and should now be moving, takeoff
        if (_targetLocation == _landingSpot.position)
        {
            isFlying = false;
            StartCoroutine(Takeoff());
            _directionToFace = directions['s'];
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
                    if (randomNum >= 0.5f)
                    {
                        _targetLocation = _northWest_Low.position;
                        _directionToFace = 180f;//face south
                    }
                    else
                    {
                        _targetLocation = _southEast_Low.position;
                        _directionToFace = 270f;//face west
                    }
                }
                else if (_targetLocation == _northWest_Low.position)
                {
                    if (randomNum >= 0.5f)
                    {
                        _targetLocation = _northEast_Low.position;
                        _directionToFace = 180f;
                    }
                    else
                    {
                        _targetLocation = _southWest_Low.position;
                        _directionToFace = 90f;
                    }
                }
                else if (_targetLocation == _southEast_Low.position)
                {
                    if (randomNum >= 0.5f)
                    {
                        _targetLocation = _northEast_Low.position;
                        _directionToFace = 270f;
                    }
                    else
                    {
                        _targetLocation = _southWest_Low.position;
                        _directionToFace = 0f;
                    }
                }
                else if (_targetLocation == _southWest_Low.position)
                {
                    if (randomNum >= 0.5f)
                    {
                        _targetLocation = _northWest_Low.position;
                        _directionToFace = directions['e'];
                    }
                    else
                    {
                        _targetLocation = _southEast_Low.position;
                        _directionToFace = directions['n'];
                    }
                }
                else
                {
                    //if the boss is at _takeoffSpot
                    if (randomNum <= 0.4f)
                    {
                        _targetLocation = _northEast_Low.position;
                    }
                    else if(randomNum <= 0.8f)
                    {
                        _targetLocation = _northWest_Low.position;
                    }
                    else if (randomNum < 0.9f)
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
                else//return back to get ready to land
                {
                    _targetLocation = _takeoffSpot.position;
                }
            }
        }//end of if Phase 1
        else if(phase == 2)// if the boss should be flying above the arena dropping bombs
        {
            if(pointsReached < 10)
            {
                if (_targetLocation == _takeoffSpot.position)
                {
                    //if the boss is at _takeoffSpot
                    if (randomNum <= 0.4f)
                    {
                        _targetLocation = _northEast_High.position;
                    }
                    else if (randomNum <= 0.8f)
                    {
                        _targetLocation = _northWest_High.position;
                    }
                    else if (randomNum < 0.9f)
                    {
                        _targetLocation = _southWest_High.position;
                    }
                    else
                    {
                        _targetLocation = _southEast_High.position;
                    }
                }
                else if (_targetLocation == _northEast_High.position)//if at northeast platform
                {
                    DropBomb();
                    if (randomNum < 0.5f)
                    {
                        _targetLocation = _northWest_High.position;
                    }
                    else
                    {
                        _targetLocation = _southEast_High.position;
                    }
                }
                else if (_targetLocation == _southEast_High.position)//if at southeast platform
                {
                    DropBomb();
                    if (randomNum < 0.5f)
                    {
                        _targetLocation = _northEast_High.position;
                    }
                    else
                    {
                        _targetLocation = _southWest_High.position;
                    }
                }
                else if (_targetLocation == _southWest_High.position)// if at southwest platform
                {
                    DropBomb();
                    if (randomNum < 0.5f)
                    {
                        _targetLocation = _northWest_High.position;
                    }
                    else
                    {
                        _targetLocation = _southEast_High.position;
                    }
                }
                else if (_targetLocation == _northWest_High.position)// if at northwest platform
                {
                    DropBomb();
                    if (randomNum < 0.5f)
                    {
                        _targetLocation = _northEast_High.position;
                    }
                    else
                    {
                        _targetLocation = _southWest_High.position;
                    }
                }
                else// if the boss is at the landing spot
                {
                    Debug.Log("Phase 2 Should be starting");
                    isFlying = false;
                    StartCoroutine(Takeoff());
                    _directionToFace = directions['s'];
                }
            }
            else//if the phase is ending
            {
                if(_targetLocation == _takeoffSpot.position)
                {
                    Land();
                    Debug.Log("Phase 2 Should be ending");
                }
                else
                {
                    _targetLocation = _takeoffSpot.position;
                }
            }
        }//End phase 2

        //Attacking Logic
        if (_targetLocation == _landingSpot.position || (_targetLocation == _takeoffSpot.position))
        {
            _attacking = false;
        }
        else if (_attacking == false && pointsReached > 1)
        {
            _attacking = true;
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
        if(Vector3.Distance(_rb.position, _targetLocation)<_movementSpeed)
        {
            _rb.position = _targetLocation;
            if (_targetLocation == _landingSpot.position)
            {
                //isFlying = false;
                Debug.Log("Target recognized as landing spot");
                GeneratePoint();
            }
            else
            {
                pointsReached++;
                GeneratePoint();
            }
        }
    }

    private void RotateToFace()
    {
        if (transform.rotation.eulerAngles.y >= 360f)
        {
            UnRotate360();
        }
        //if not facing the right direction
        float _currentFacing = transform.rotation.eulerAngles.y;
        float _amountToTurn = _directionToFace - _currentFacing;
        if (Mathf.Abs(_amountToTurn) > _rotationSpeed)
        {
            //TODO Add Rotation logic that stops 270 degree turns
            float _rotateAmount = _rotationSpeed * (_amountToTurn / 30);
            transform.Rotate(0f, _rotateAmount, 0f);
        }
    }

    private void UnRotate360()
    {
        transform.Rotate(0f, -360f, 0f);
    }

    private bool Phase3Check()
    {
        return false;
        /*
        if(_bossHealth.GetHealth() <= 3)
        {
            return true;
        }
        else
        {
            return false;
        }
        */
    }

    private void Shoot()
    {
        _wb.Fire();
    }

    private void DropBomb()
    {
        _bombSystem.Fire();
    }

    void UpdateHealthbar()
    {
        _bossHealthbar.value = _bossHealth.GetHealth();
    }

    //Dictionary that contains Vector3 values associated with key points
    //Dictionary<string, Vector3> movementPoints = new Dictionary<string, Vector3>();
}