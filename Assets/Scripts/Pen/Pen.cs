using UnityEngine;

public class Pen : MonoBehaviour
{
    [SerializeField] private ParticleSystem levelCompleteParticleFX;

    private const string SHEEP_TAG = "Sheep";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(SHEEP_TAG))
        {
            levelCompleteParticleFX.Play();
            LevelManager.Instance.CompleteLevel();
        }
    }
}
