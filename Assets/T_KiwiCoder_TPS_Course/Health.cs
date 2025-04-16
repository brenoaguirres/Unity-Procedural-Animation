using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    float _maxHealth = 300f;
    float _currentHealth;

    TextBoxBillboard _textBoxBillboard;

    void Start()
    {
       _currentHealth = _maxHealth;
       _textBoxBillboard = GetComponentInChildren<TextBoxBillboard>(true);
    }

    void EmitParticle()
    {

    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damageValue)
    {
        _currentHealth -= damageValue;
        EmitParticle();
        _textBoxBillboard.ShowDamage(damageValue);

        if (_currentHealth <= 0)
            Die();
    }
}
