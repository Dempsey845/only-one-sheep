using System.Collections;
using UnityEngine;

public class PlayButtonDestructable : MonoBehaviour
{
    [SerializeField] private GameObject playText;
    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material hoveredMaterial;

    private bool collided = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collided || !other.CompareTag("Sheep")) { return; }
        playText.SetActive(false);
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
