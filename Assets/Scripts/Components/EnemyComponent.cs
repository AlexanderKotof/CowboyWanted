using UnityEngine;
using UnityEngine.AI;

public class EnemyComponent : CharacterComponent
{
    public NavMeshAgent agent;
    public Collider collider;

    private PlayerComponent _player;

    public float moveSpeed = 1;

    public float damage = 5;
    public float attackDistance = 1;

    public float attackTime = 1;

    public int reward = 1;

    private float _lastAttackTime;

    protected override void Start()
    {
        base.Start();
        agent.avoidancePriority = UnityEngine.Random.Range(0, 100);
        agent.speed = moveSpeed;

        Died += EnemyComponent_Died;
    }

    private void EnemyComponent_Died(CharacterComponent obj)
    {
        agent.isStopped = true;
        agent.enabled = false;
        collider.enabled = false;
    }

    public void SetDestination(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void Update()
    {
        if ((_player.transform.position - transform.position).sqrMagnitude > attackDistance * attackDistance)
        {
            SetDestination(_player.transform.position - transform.forward * attackDistance * 0.9f);
            return;
        }

        if (_lastAttackTime + attackTime > Time.realtimeSinceStartup)
            return;

        SetDestination(transform.position);

        _lastAttackTime = Time.realtimeSinceStartup;

        _player.TakeDamage(damage);
    }

    public void SetPlayer(PlayerComponent player)
    {
        _player = player;
    }
}
