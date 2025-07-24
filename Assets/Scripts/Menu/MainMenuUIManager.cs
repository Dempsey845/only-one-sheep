using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private LayerMask clickLayer;
    [SerializeField] private GameObject sheep;
    [SerializeField] private PlayButtonDestructable playButtonDestructable;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, clickLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                sheep.SetActive(true);
            }

            playButtonDestructable.Hover();
        }
        else
        {
            playButtonDestructable.UnHover();
        }
    }
}
