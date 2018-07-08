using UnityEngine;

public class VFXKiller : MonoBehaviour
{
    [Header("Particle settings")]
    [SerializeField]
    protected float destroyDelay = 0f;
    [SerializeField]
    protected bool isTheParticleAAnimation;

    [Header("Particle components")]
    [SerializeField]
    protected ParticleSystem particle;
    [SerializeField]
    protected Animator animationParticle;

    protected float startTime;

    protected virtual void Awake()
    {
        if (animationParticle == null && isTheParticleAAnimation == true)
            animationParticle = GetComponent<Animator>();
        if (particle == null && isTheParticleAAnimation == false)
            particle = GetComponent<ParticleSystem>();

        return;
    }

    protected virtual void Start()
    {
        if (destroyDelay != 0)
        {
            startTime = Time.timeSinceLevelLoad;
        }

        return;
    }

    protected virtual void Update()
    {
        if ((destroyDelay != 0) && (Time.timeSinceLevelLoad - startTime < destroyDelay))
        {
            return;
        }

        if (isTheParticleAAnimation == true && animationParticle != null)
        {
            if (animationParticle.GetCurrentAnimatorStateInfo(0).length + startTime > Time.timeSinceLevelLoad)
            {
                return;
            }
        }
        else if (isTheParticleAAnimation == false && particle != null)
        {
            if (particle.isPlaying)
            {
                return;
            }
        }

        DestroyParticleSystem();
    }

    protected virtual void DestroyParticleSystem()
    {
        Destroy(gameObject);
        return;
    }
}
