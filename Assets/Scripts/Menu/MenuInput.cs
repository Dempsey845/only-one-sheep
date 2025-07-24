using System;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    [SerializeField] private MenuManager menuManager;

    public event Action OnGoBack;

    private void Update()
    {
        if (PlayerInputManager.Instance != null && PlayerInputManager.Instance.MenuPressed)
        {
            if (!menuManager.IsOpened)
            { 
                menuManager.gameObject.SetActive(true);
            } else
            {
                OnGoBack?.Invoke();
            }
        }
    }
}
