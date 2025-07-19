using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        if (PlayerManager.Instance != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}
