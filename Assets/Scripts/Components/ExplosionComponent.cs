using UnityEngine;

public class ExplosionComponent : MonoBehaviour
{
    public ParticleSystem particles;

    private void Start()
    {
        particles.Play();
        Invoke("Hide", particles.main.duration);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
