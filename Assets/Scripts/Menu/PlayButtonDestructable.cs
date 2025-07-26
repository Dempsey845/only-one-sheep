using System.Collections;
using UnityEngine;

public class PlayButtonDestructable : MonoBehaviour
{
    [SerializeField] private GameObject playText;
    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material hoveredMaterial;
    [SerializeField] private AudioSource musicSource;

    private bool collided = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (collided)
        {
            musicSource.volume -= Time.deltaTime / 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collided || !other.CompareTag("Sheep")) { return; }
        playText.SetActive(false);
        audioSource.Play();
        StartCoroutine(StartGame());
        collided = true;
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3.5f);
        LevelTransitionManager.Instance.TransitionOutOfScene(1);
    }

    public void Hover()
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material = hoveredMaterial;
        }
    }

    public void UnHover()
    {
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material = defaultMaterial;
        }
    }
}
