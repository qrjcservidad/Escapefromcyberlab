using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float laserDamage = 20f;

    [Header("UI References")]
    public Slider healthSlider;
    public GameObject gameOverPanel;
    public Image damageOverlay;             // Drag ang DamageOverlay UI Image dito

    [Header("Damage Effect Settings")]
    public Color flashColor = new Color(1f, 0f, 0f, 0.4f); // Pulang opacity kapag nadamage
    public float flashSpeed = 5f;                       // Bilis ng pagkawala ng red flash

    [Header("Animation Settings")]
    public Animator playerAnimator;         // Drag Animator/Player Model dito
    public string deathTriggerName = "Die"; // Pangalan ng Trigger parameter sa Animator
    public float gameOverDelay = 5.0f;      // Oras bago lumabas ang Game Over Panel (para matapos ang death animation)

    private bool isDead = false;
    private bool isFlashing = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Kusa nitong hahanapin ang Animator sa GameObject o sa children kung hindi na-assign sa Inspector
        if (playerAnimator == null)
        {
            playerAnimator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        // Kapag tinamaan ng laser, mabilis na ipa-fade out ang red flashOverlay pabalik sa clear/transparent
        if (damageOverlay != null)
        {
            if (isFlashing)
            {
                damageOverlay.color = flashColor;
                isFlashing = false;
            }
            else
            {
                damageOverlay.color = Color.Lerp(damageOverlay.color, Color.clear, flashSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        // Check if player hits object tagged "Laser"
        if (other.CompareTag("Laser"))
        {
            TakeDamage(laserDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // I-trigger ang Screen Flash Effect
        isFlashing = true;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // 1. Play Death Animation (subukan muna ang Trigger, gagamit ng direct Play bilang fallback)
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger(deathTriggerName);
            playerAnimator.Play(deathTriggerName, 0, 0f);
        }

        // 2. Disable Player Controls (halimbawa: Movement o Interaction scripts para hindi na makagalaw ang player habang namamatay)
        MonoBehaviour movementScript = GetComponent("PlayerMovement") as MonoBehaviour; // Palitan sa totoong name ng movement script mo kung mayroon
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // 3. Simulan ang Delay Coroutine bago ipakita ang Game Over UI at i-pause ang time
        StartCoroutine(HandleGameOverSequence());
    }

    private IEnumerator HandleGameOverSequence()
    {
        // Maghihintay muna habang nagpe-play ang death animation
        yield return new WaitForSeconds(gameOverDelay);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Unlock cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pause time pagkatapos ng animation
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume time before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}