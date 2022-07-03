using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2 _mouseSensetivity = Vector2.one;
    [SerializeField] private Vector3 _targetOffset = Vector2.zero;

    [Header("States")]
    [SerializeField] private float _stateTransitionSpeed = 1f;
    [SerializeField] private Vector3 _defaultState = Vector3.zero;

    [Space]
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _cameraInnerTransform;

    public Transform target;
    
    public Quaternion TargetRotation { get; private set; }
    public Vector3 TargetRotationEuler { get; private set; }
    public Quaternion CurrentRotation { get; private set; }
    public Vector3 CurrentState { get; private set; }

    public static CameraController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        SetDefaultState();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (target == null)
            return;
        GetInput();

        UpdatePosition();
        UpdateRotation();
        UpdateState();
    }

    private void Init()
    {
        TargetRotation = _center.rotation;
        TargetRotationEuler = _center.rotation.eulerAngles;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void GetInput()
    {
        Vector2 input = InputHandler.Instance.MouseInput;
        input *= _mouseSensetivity;

        var rotEuler = TargetRotationEuler;
        rotEuler += new Vector3(-input.y, input.x, 0f);
        rotEuler.x = Mathf.Clamp(rotEuler.x, -60f, 80f);
        TargetRotationEuler = rotEuler;

        TargetRotation = Quaternion.Euler(TargetRotationEuler);
    }

    private void UpdatePosition()
    {
        _center.position = target.position + _targetOffset;
    }

    private void UpdateRotation()
    {
        CurrentRotation = TargetRotation;
        _center.rotation = CurrentRotation;
    }

    private void UpdateState()
    {
        _cameraInnerTransform.localPosition
            = Vector3.Lerp(_cameraInnerTransform.localPosition, CurrentState, Time.deltaTime * _stateTransitionSpeed);
    }

    public void SetDefaultState()
    {
        CurrentState = _defaultState;
    }
}
