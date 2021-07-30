using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState
{
    Inactive,
    Moving,
    Paused
}

public class MoveBetweenPoints : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Collider2D _objectCollider;
    [SerializeField]
    [Tooltip("Destination transform to move towards")]
    private Transform _endLocation;

    [Header("Movement Settings")]
    [SerializeField][Tooltip("Seconds from start to end location")]
    private float _secondsUntilDestination;
    [SerializeField][Tooltip("Duration in seconds to pause after reaching destination")]
    private float _pauseDuration;

    [Header("General Settings")]
    [SerializeField][Tooltip("True will begin movement on scene start. Disable if you'd like to control" +
        " the start movement with a trigger event")]
    private bool _activateOnAwake = true;
    [SerializeField][Tooltip("Duration before starting movement. Use this to stagger timing while" +
        " retaining the desired speed")]
    private float _startDelay = 0;

    private MoveState _moveState = MoveState.Inactive;

    private int _tripCounter = 1;
    private float _movingElapsedTime;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Coroutine _moveRoutine;
    // we need to save our movement change so we can carry other objects
    public Vector2 PreviousPosition { get; private set; }
    public Vector2 Velocity => (_rb.position - PreviousPosition) / Time.fixedDeltaTime;

    private void Awake()
    {
        _rb.isKinematic = true;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        _startPosition = _rb.position;
        // error-check our end location
        if (_endLocation != null)
            _endPosition = _endLocation.position;
        else
        {
            _endPosition = _rb.position;
            Debug.LogWarning("No end location set on moving platform");
        }
        // always start in forward direction

        if (_activateOnAwake)
        {
            Activate();
        }
    }

    public void Activate()
    {
        _moveRoutine = StartCoroutine(MoveRoutine(_secondsUntilDestination));
    }

    public void Stop()
    {
        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);
        _moveState = MoveState.Inactive;
    }

    void Update()
    {
        //If the platform is moving, make sure we're counting it to the timer
        if (_moveState == MoveState.Moving)
        {
            // this only counts time moving, to ensure we're counting properly
            _movingElapsedTime += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        PreviousPosition = _rb.position;
    }

    IEnumerator MoveRoutine(float secondsUntilDestination)
    {
        // check for start delay before beginning our loop
        if(_startDelay >= 0)
        {
            _moveState = MoveState.Paused;
            yield return new WaitForSeconds(_startDelay);
        }

        _moveState = MoveState.Moving;
        while (true)
        {
            switch (_moveState)
            {
                case MoveState.Moving:
                    Move(secondsUntilDestination);
                    break;
                case MoveState.Paused:
                    yield return new WaitForSeconds(_pauseDuration);
                    _moveState = MoveState.Moving;
                    break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void Move(float secondsUntilDestination)
    {
        float moveRatio = Mathf.PingPong(_movingElapsedTime / secondsUntilDestination, 1f);
        // saving previous position allows us to calculate the move vector for objects that need to be carried

        Vector2 newPosition = Vector2.Lerp(_startPosition, _endPosition,
            Mathf.SmoothStep(0f, 1f, moveRatio));

        _rb.MovePosition(newPosition);

        if ((_movingElapsedTime / _tripCounter) >= _secondsUntilDestination)
        {
            _moveState = MoveState.Paused;
            _tripCounter++;
        }
    }

    private void OnDrawGizmos()
    {
        if(_objectCollider == null || _endLocation == null) { return; }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_endLocation.position, _objectCollider.bounds.size);
    }
}
