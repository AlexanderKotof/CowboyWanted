using System;
using UnityEngine;

public class CharacterComponent : MonoBehaviour
{
    public float startHealth = 10;
    private float currentHealth;

    public bool IsDied => currentHealth <= 0;

    public event Action<float> HealthChanged;
    public event Action<CharacterComponent> Died;

    public Animator animator;


    protected virtual void Start()
    {
        currentHealth = startHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDied)
            return;

        currentHealth -= damage;
        HealthChanged?.Invoke(currentHealth);

        if (IsDied)
        {
            Die();
        }
    }

    private void Die()
    {
        Died?.Invoke(this);
        animator.SetTrigger("Die");
    }
}
