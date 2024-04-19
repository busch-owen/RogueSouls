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
    
    protected override void Awake()
    {
        base.Awake();
        _baseDashRate = _dashRate;
        
        if(!_rb) _rb = gameObject.AddComponent<Rigidbody2D>();
        
        agent = GetComponent<NavMeshAgent>();
        _dashTrail = GetComponentInChildren<TrailRenderer>();
        _dashTrail.enabled = false;
        _isPhaseTwo = false;
        _rb.simulated = false;
        _rb.gravityScale = 0f;
        agent.enabled = true;
        InvokeRepeating(nameof(Dash), _dashRate, _dashRate);
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
        
        _isDashing = true;
        _canDash = false;
        _rb.simulated = true;
        Vector3 dashVector = agent.velocity;
        agent.enabled = false;
        _dashTrail.enabled = true;
        
        _rb.AddForce(dashVector.normalized * _dashForce);
        Debug.Log("Dashed");
        Invoke(nameof(ResetDashState), _invulnTime);
    }

    private void ResetDashState()
    {
        _isDashing = false;
        _canDash = true;
        _rb.simulated = false;
        agent.enabled = true;
        _dashTrail.enabled = false;
    }
}
