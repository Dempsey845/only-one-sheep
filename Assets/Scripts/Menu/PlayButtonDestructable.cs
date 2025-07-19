using System.Collections;
using UnityEngine;

public class PlayButtonDestructable : MonoBehaviour
{
    [SerializeField] private GameObject playText;

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
}
