using UnityEngine;
using UnityEngine.UI;

public class SheepHealthBar : MonoBehaviour
{
    [SerializeField] private SheepHealth sheepHealth;

    [SerializeField] private Slider slider;
    [SerializeField] private Slider easeSlider;

    [SerializeField] private float lerpSpeed = 0.05f;

    private void Start()
    {
        slider.maxValue = sheepHealth.GetMaxHealth();
        slider.value = sheepHealth.CurrentHealth;

        easeSlider.maxValue = sheepHealth.GetMaxHealth();
        easeSlider.value = sheepHealth.CurrentHealth;
    }

    private void Update()
    {
        if (slider.value != easeSlider.value)
        {
            easeSlider.value = Mathf.Lerp(easeSlider.value, sheepHealth.CurrentHealth, lerpSpeed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        sheepHealth.OnDamaged += HandleDamaged;
        sheepHealth.OnHealed += HandleHealed;
        sheepHealth.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void UnSubscribeEvents()
    {
        sheepHealth.OnDamaged -= HandleDamaged;
        sheepHealth.OnHealed -= HandleHealed;
        sheepHealth.OnDied -= HandleDied;
    }

    private void HandleDamaged(int currentHealth)
    {
        UpdateSlider();
    }

    private void HandleHealed(int currentHealth)
    {
        UpdateSlider();
    }

    private void HandleDied()
    {
        UnSubscribeEvents();
        gameObject.SetActive(false);
    }

    private void UpdateSlider()
    {
        slider.value = sheepHealth.CurrentHealth;
    }


}
