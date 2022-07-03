using System.Collections.Generic;
using PlayerStateMachine;
using UnityEngine.Events;
using UnityEngine;
using Mirror;
using Skills;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(NetworkTransform))]
public class Player : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _maxSpeed = 1f;
    [SerializeField] private float _acceleration = 1f;

    [Space]
    [SerializeField] private Skill _skill;

    [SyncVar]
    private uint _score;
    [SyncVar]
    private float _elapsedAfterSkillApplying;

    private NetworkTransform _networkTransform;
    private StateMachine _stateMachine;
    private Animator _animator;
    private Material _material;

    [SyncVar]
    public string playerName;

    public uint Score => _score;
    public State CurrentState => _stateMachine.CurrentState;
    public Vector2 MoveInput { get; set; }
    public Vector2 CurrentSpeed { get; private set; }

    public event UnityAction<Skill> SkillApplied;
    public event UnityAction<Skill> SkillElapsed;
    public event UnityAction Hitted;

    public void SetColor(Color color)
    {
        _material.color = color;
    }

    public override void OnStartLocalPlayer()
    {
        InputHandler.Instance.SetPlayer(this);
        InputHandler.Instance.LeftMouseButtonPerformed += OnLMBPerformed;
        CameraController.Instance.target = transform;
    }

    private void Awake()
    {
        Init();
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer && isClient == false)
            return;

        UpdateMovement();
        
        _elapsedAfterSkillApplying += Time.fixedDeltaTime;
    }

    [ServerCallback]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var hittedPlayer))
        {
            if (hittedPlayer.CurrentState.GetType() == typeof(DashState) && CurrentState.GetType() != typeof(InvulnerableState))
            {
                Hitted?.Invoke();
                RpcOnHitted();
            }
            if (CurrentState.GetType() == typeof(DashState) && hittedPlayer.CurrentState.GetType() != typeof(InvulnerableState))
            {
                _score += 1;
            }
        }
    }

    private void OnDestroy()
    {
        MatchManager.Instance.MatchRestarting -= OnMatchRestarting;
    }

    [ClientRpc]
    private void RpcOnHitted() => Hitted?.Invoke();

    private void Init()
    {
        _stateMachine = GetComponent<StateMachine>();
        _networkTransform = GetComponent<NetworkTransform>();
        MatchManager.Instance.OnPlayerSpawned(this);
        MatchManager.Instance.MatchRestarting += OnMatchRestarting;
        _networkTransform.clientAuthority = true;
        GetAndSetSharedMaterial();
    }

    private void GetAndSetSharedMaterial()
    {
        List<Renderer> renderers = new List<Renderer>();
        GetComponentsInChildren(renderers);
        _material = new Material(renderers[0].sharedMaterial);

        foreach (var renderer in renderers)
            renderer.sharedMaterial = _material;
    }

    private void UpdateMovement()
    {
        bool isMoving = CurrentSpeed.sqrMagnitude > 0.5f;
        bool isBraking = MoveInput.y < 0 && isMoving == true;
        //bool canGoReverse = isMoving == false && isBraking == true;

        var inputRotatedWorld = new Vector3(MoveInput.x, 0f, MoveInput.y);
        inputRotatedWorld = transform.rotation * inputRotatedWorld;
        var inputRotated = new Vector2(inputRotatedWorld.x, inputRotatedWorld.z);
        inputRotated.Normalize();

        if (isBraking == false)
            CurrentSpeed = CurrentSpeed.Slerp2d(inputRotated * _maxSpeed, Time.fixedDeltaTime * _acceleration);
        else
            CurrentSpeed = Vector2.Lerp(CurrentSpeed, Vector2.zero, Time.fixedDeltaTime * _acceleration);

        var movement = new Vector3(CurrentSpeed.x, 0f, CurrentSpeed.y);
        movement *= Time.fixedDeltaTime;

        transform.position += movement;

        if (isMoving)
            transform.forward = movement;

        var localSpeed3 = new Vector3(CurrentSpeed.x, 0f, CurrentSpeed.y);
        localSpeed3 = Quaternion.Inverse(transform.rotation) * localSpeed3;
        var localSpeed = new Vector2(localSpeed3.x, localSpeed3.z);
        localSpeed.Normalize();
    }


    private void OnSkillElapsed(Skill skill)
    {
        SkillElapsed?.Invoke(skill);

        _skill.EffectElapsed -= OnSkillElapsed;
    }

    private void OnMatchRestarting()
    {
        _score = 0;
    }

    private void OnLMBPerformed()
    {
        if (_elapsedAfterSkillApplying >= _skill.Cooldown)
        {
            _elapsedAfterSkillApplying = 0;
            SkillApplied?.Invoke(_skill);
            _skill.Apply(this);
            _skill.EffectElapsed += OnSkillElapsed;
        }
    }
}
