using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlaneController : MonoBehaviour
{
    [Header("Atributtes")]
    [SerializeField] private int life;
    [SerializeField] private int maxLife;
    [SerializeField] private int points;
    private  float startTime;

    public int Life => life;
    public int MaxLife => maxLife;
    public int Points => points;

    [Header("Position")]
    [SerializeField] float MaxHorizontalPosition;
    [SerializeField] float MinHorizontalPosition;
    [SerializeField] float MaxVerticalPosition;
    [SerializeField] float MinVerticalPosition;
    Vector3 clampedPosition;

    [Header("Controls Properties")]
    [SerializeField] private float pitchPlane;
    [SerializeField] private float pitchGain = 1f;
    [SerializeField] private MinMax pitchTreshHold;
    [SerializeField] private float rollPlane;
    [SerializeField] private float rollhGain = 1f;
    [SerializeField] private MinMax rollTreshHold;

    [Header("Rotation Data")]
    [SerializeField] private Quaternion qx = Quaternion.identity; //<0,,0,0,1>
    [SerializeField] private Quaternion qy = Quaternion.identity; //<0,,0,0,1>
    [SerializeField] private Quaternion qz = Quaternion.identity; //<0,,0,0,1>

    [SerializeField] private Quaternion r = Quaternion.identity; //<0,,0,0,1>
    private float anguloSen;
    private float anguloCos;

    protected float _pitchDirection = 0f;
    protected float _rollDirection = 0f;

    bool isStopMovement;

    public static event Action OnPlayerTakeTrush;
    public static event Action OnPlayerTakeMeteor;
    public static event Action OnPlayerDeadth;
    public static event Action OnPlayerGetPointsForWin;

   
    private void Update()
    {
        AddPoints();
        if (life <= 0)
        {
            OnPlayerDeadth?.Invoke();

        }

    }

    private void FixedUpdate()
    {
        pitchPlane += _pitchDirection * pitchGain;

        pitchPlane = Mathf.Clamp(pitchPlane, pitchTreshHold.MinValue, pitchTreshHold.MaxValue);

        rollPlane += _rollDirection * rollhGain;

        rollPlane = Mathf.Clamp(rollPlane, rollTreshHold.MinValue, rollTreshHold.MaxValue);

        //rotacion z -> x -> y
        anguloSen = Mathf.Sin(Mathf.Deg2Rad * rollPlane * 0.5f);
        anguloCos = Mathf.Cos(Mathf.Deg2Rad * rollPlane * 0.5f);
        qz.Set(0, 0, anguloSen, anguloCos);

        anguloSen = Mathf.Sin(Mathf.Deg2Rad * pitchPlane * 0.5f);
        anguloCos = Mathf.Cos(Mathf.Deg2Rad * pitchPlane * 0.5f);
        qx.Set(anguloSen, 0, 0, anguloCos);

        /*anguloSen = Mathf.Sin(Mathf.Deg2Rad * rollPlane * 0.5f);
        anguloCos = Mathf.Cos(Mathf.Deg2Rad * rollPlane * 0.5f);
        qy.Set(0, anguloSen, 0, anguloCos);*/

        //multiplicación y -> x -> z
        r = qy * qx * qz;

        transform.rotation = r;

        UpdatePosition();
        if (isStopMovement)
        {
            RestorePosition();
        }

    }

    private void RestorePosition()
    {
        if (pitchPlane < 0)
        {
            pitchPlane += pitchGain;
            pitchPlane = Mathf.Clamp(pitchPlane, pitchTreshHold.MinValue, 0);
        }

        if (pitchPlane > 0)
        {
            pitchPlane += -pitchGain;
            pitchPlane = Mathf.Clamp(pitchPlane, 0, pitchTreshHold.MaxValue);
        }

        if (rollPlane < 0)
        {
            rollPlane += rollhGain;
            rollPlane = Mathf.Clamp(rollPlane, rollTreshHold.MinValue, 0);
        }

        if (rollPlane > 0)
        {
            rollPlane += -rollhGain;
            rollPlane = Mathf.Clamp(rollPlane, 0, rollTreshHold.MaxValue);
        }

        anguloSen = Mathf.Sin(Mathf.Deg2Rad * rollPlane * 0.5f);
        anguloCos = Mathf.Cos(Mathf.Deg2Rad * rollPlane * 0.5f);
        qz.Set(0, 0, anguloSen, anguloCos);

        anguloSen = Mathf.Sin(Mathf.Deg2Rad * pitchPlane * 0.5f);
        anguloCos = Mathf.Cos(Mathf.Deg2Rad * pitchPlane * 0.5f);
        qx.Set(anguloSen, 0, 0, anguloCos);
        r = qy * qx * qz;

        transform.rotation = r;

    }


    //Pitch -> X Axis
    public void RotatePitch(InputAction.CallbackContext context)
    {
        _pitchDirection = context.ReadValue<float>();
    }

    //Roll -> Z Axis
    public void RotateRoll(InputAction.CallbackContext context)
    {
        _rollDirection = context.ReadValue<float>();
    }


    private float _verticalDirection = 0f;
    private float _horizontalDirection = 0f;
    [SerializeField] private float velocitySpeed = 5f;

    private Rigidbody _myRB;

    private void Start()
    {
        _myRB = GetComponent<Rigidbody>();
        life = maxLife;
        startTime = Time.time;
    }

    public void TranslateVertical(InputAction.CallbackContext context)
    {
        _verticalDirection = context.ReadValue<float>();
    }

    public void TranslateHorizontal(InputAction.CallbackContext context)
    {
        _horizontalDirection = context.ReadValue<float>();
    }

    private void UpdatePosition()
    {

        _myRB.linearVelocity = new Vector3(_horizontalDirection * velocitySpeed, _verticalDirection * velocitySpeed, 0f);


        clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, MinHorizontalPosition, MaxHorizontalPosition);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, MinVerticalPosition, MaxVerticalPosition);
        transform.position = clampedPosition;

        if (_myRB.linearVelocity == Vector3.zero)
        {
            isStopMovement = true;
        }
        else
        {
            isStopMovement = false;
        }
    }
    void AddPoints()
    { 
        points = (int)(Time.time - startTime);
     if(points >= 20)
        {
            OnPlayerGetPointsForWin?.Invoke();  
        }

    }

  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trush"))
        {
            life--;
            OnPlayerTakeTrush?.Invoke();

            Destroy(other.gameObject);
        }
        if (other.CompareTag("Meteor"))
        {
            life--;
            OnPlayerTakeMeteor?.Invoke();
            Destroy(other.gameObject);
        }
    }



}

    [System.Serializable]
    public struct MinMax
    {
        public float MinValue;
        public float MaxValue;
    }



 
    
