using UnityEngine;
using UnityEngine.AI;

public class UncleBoss : KingSlime
{
    private Rigidbody2D _rb;

    private bool _isDashing;
    private bool _canDash = true;

    private TrailRenderer _dashTrail;

    [SerializeField] private float _dashForce;
    [SerializeField] private float _invulnTime;
    [SerializeField] private float _dashRate;
    private float _baseDashRate;

    private bool _isPhaseTwo;

    private Animator _anim;

    private LayerMask _normalMask;
    private LayerMask _invulnerableMask;
    
    
    private static readonly int Dashing = Animator.StringToHash("Dashing");

    protected override void Awake()
    {
        base.Awake();
        _baseDashRate = _dashRate;
        
        _normalMask = LayerMask.NameToLayer("UncleEnemy");
        _invulnerableMask  = LayerMask.NameToLayer("Invulnerable");

        if (!_anim) _anim = GetComponentInChildren<Animator>();
        if(!_rb) _rb = gameObject.AddComponent<Rigidbody2D>();
        
        _agent = GetComponent<NavMeshAgent>();
        _dashTrail = GetComponentInChildren<TrailRenderer>();
        
        ResetDashState();
        
        InvokeRepeating(nameof(Dash), _dashRate, _dashRate);
    }

    protected override void Update()
    {
        if (!_isDashing)
        {
            enemyGun.Shoot();
            RangedAttack();
        }
        base.Update();
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _dashRate = _baseDashRate;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (Health > _maxHealth / 2 || _isPhaseTwo) return;
        
        _isPhaseTwo = true;
        _dashRate *= 0.5f;
    }

    private void Dash()
    {
        if (_isDashing || !_canDash) return;
        
        gameObject.layer = _invulnerableMask;
        _anim.SetTrigger(Dashing);
        _rb.isKinematic = false;
        _isDashing = true;
        _canDash = false;
        var dashVector = _agent.velocity.normalized;
        _agent.enabled = false;
        _dashTrail.enabled = true;
        
        _rb.AddForce(dashVector * _dashForce);
        Invoke(nameof(ResetDashState), _invulnTime);
    }

    private void ResetDashState()
    {
        gameObject.layer = _normalMask;
        _rb.isKinematic = true;
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.zero;
        _isDashing = false;
        _canDash = true;
        _agent.enabled = true;
        _dashTrail.enabled = false;
    }
}
