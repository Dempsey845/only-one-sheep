using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WalkParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem walkParticleSystem;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private Transform terrainCheckPoint;
    [SerializeField] private float terrainCheckLength = 0.2f;

    private PlayerMovement playerMovement;
    private EmissionModule emissionModule;
    private TerrainColorSampler terrainColorSampler;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        terrainColorSampler = GetComponent<TerrainColorSampler>();
        emissionModule = walkParticleSystem.emission;
    }

    private void Update()
    {
        bool isOnTerrain = Physics.Raycast(terrainCheckPoint.position, Vector3.down, out RaycastHit hit, terrainCheckLength, terrainLayer);
        bool shouldEmit = playerMovement.IsSprinting && isOnTerrain;

        emissionModule.enabled = shouldEmit;

        if (shouldEmit && hit.collider.TryGetComponent(out Terrain terrain))
        {
            terrainColorSampler.SetTerrain(terrain);
        }
    }
}
