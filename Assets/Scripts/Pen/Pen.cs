using UnityEngine;

public class Pen : MonoBehaviour
{
    private const string SHEEP_TAG = "Sheep";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(SHEEP_TAG))
        {
            Debug.Log("Level Complete!");
        }
    }
}
