// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum HealthEventType {HealthLoss, HealthDepleted, HealthGain}
public class HealthEvent : GameEvent
{
    private HealthEventType eventType;
    private GameObject gameObjectWithHealth;
    public HealthEventType GetEventType{get { return eventType; } }
    public GameObject GetGameObjectWithHealth { get { return gameObjectWithHealth; } }

    public HealthEvent(HealthEventType _eventType, GameObject _gameObjetWithHealth)
    {
        eventType = _eventType;
        gameObjectWithHealth = _gameObjetWithHealth;
        return;
    }
}

public class Health : MonoBehaviour, Damageable
{
    [Header("Health settings")]
    [SerializeField] private float maxHealth;
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private float healthBarToggleTime;

    [Header("Health components")]
    [SerializeField] private GameObject damageVfx;
    [SerializeField] private AudioClip damageSfx;
    [SerializeField] private GameObject deathVfx;

	[Header("Health editor debugging")]
    [SerializeField] private float currentHealth;

    protected void Start()
    {
        currentHealth = maxHealth;
        return;
    }

    protected void OnEnable()
    {
        currentHealth = maxHealth;
        if(healthBar != null)
        {
            UpdateHealthBar();
            healthBar.gameObject.SetActive(false);
        }
        return;
    }

    private void UpdateHealthBar()
    {
        if(healthBar != null) {
            healthBar.UpdateBar(currentHealth, maxHealth);
        }
        return;
    }

    public void TakeDamage(float _damage)
    {
        // We are laready dead.
        if(currentHealth == 0) { return; }
        StartCoroutine(ToggleHealthBar());
        currentHealth -= _damage;
        VisualEffects.CreateVisualEffect(damageVfx, transform);
        PlayHitSfx();
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateHealthBar();
        if (currentHealth == 0)
            StartCoroutine(Kill());
        return;
    }

    private IEnumerator ToggleHealthBar()
    {
        if(CanToggleHealthBar())
        {
            healthBar.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(healthBarToggleTime);
            healthBar.gameObject.SetActive(false);
        }
        yield break;
    }

    private bool CanToggleHealthBar()
    {
        bool canToggle = true;
        try
        {
            if(healthBarToggleTime == 0) {
                canToggle = false;
            }
            if(healthBar == null) 
            {
                canToggle = false;
                throw new MissingComponentException(gameObject, typeof(Health));
            }
        }
        catch (MissingComponentException _missingComponentException) {
            _missingComponentException.DisplayException();
        }
        return canToggle;
    }

    private void PlayHitSfx()
    {
        /*if (damageSfx != null)
        {
            SoundManager.Instance.PlaySfx(character.CharacterAudioSource, damageSfx);
        }
        return;*/
    }

    public IEnumerator Kill()
    {
        VisualEffects.CreateVisualEffect(deathVfx, transform);
		EventManager.TriggerEvent(new HealthEvent(HealthEventType.HealthDepleted, gameObject));
        yield break;
    }
}
