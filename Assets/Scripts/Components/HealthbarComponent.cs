using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarComponent : MonoBehaviour
{
    public Slider healthbar;
    public TMP_Text damageText;

    public CharacterComponent linkedCharacter;

    private Transform _mainCameraTransform;
    private Coroutine coroutine;

    private Vector3 _textPosition;

    void Start()
    {
        _mainCameraTransform = Camera.main.transform;

        healthbar.maxValue = linkedCharacter.startHealth;
        healthbar.value = linkedCharacter.startHealth;

        linkedCharacter.HealthChanged += OnHealthChanged;

        damageText.SetText("");
        _textPosition = damageText.rectTransform.localPosition;
    }

    private void OnDestroy()
    {
        linkedCharacter.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(float health)
    {
        var change = healthbar.value - health;
        healthbar.value = health;

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(ShowDamageText(change));

        if (health <= 0)
            healthbar.gameObject.SetActive(false);
    }

    private IEnumerator ShowDamageText(float damage)
    {
        var color = damageText.color;
        damageText.SetText(Mathf.CeilToInt(damage).ToString());
        color.a = 0;

        damageText.rectTransform.localPosition = _textPosition;

        while (color.a < 1)
        {
            color.a += Time.deltaTime * 2;
            damageText.color = color;

            damageText.rectTransform.localPosition = Vector3.MoveTowards(damageText.rectTransform.localPosition, damageText.rectTransform.localPosition + Vector3.up, Time.deltaTime * 2);
            yield return null;
        }
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * 5;
            damageText.color = color;
            yield return null;
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(_mainCameraTransform.position - transform.position, Vector3.up);
    }
}