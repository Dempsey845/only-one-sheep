using UnityEngine;

public class FoxManager : MonoBehaviour
{
    public Transform FoxHome { get; private set; }

    public FoxHealth FoxHealth { get; private set; }

    public void Init(Transform foxHome)
    {
        FoxHome = foxHome;
        GetComponent<FoxStateController>().Init(foxHome);
        FoxHealth = GetComponent<FoxHealth>();
    }
}