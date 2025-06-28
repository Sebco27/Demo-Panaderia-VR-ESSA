using UnityEngine;

public class PlateMassController : MonoBehaviour
{
    [Header("Prefab de la masa y punto de spawn")]
    public GameObject masaPrefab;
    public Transform spawnPoint;
    public AudioSource doughSound;
    public AudioSource eggSound;
    public AudioSource milkSound;
    public AudioSource flourSound;

    [Header("Escalas")]
    public Vector3 initialScale = Vector3.one * 0.2f;     // tamaño tras 1 ingrediente
    public Vector3 scaleStep    = Vector3.one * 0.3f;     // cuánto crece por ingrediente

    private GameObject masaInstance;
    private int ingredientCount = 0;
    private const int maxIngredients = 3;

    /// <summary>
    /// Llamarás a este método desde cada socket en SelectEntered
    /// para notificar que has añadido un ingrediente.
    /// </summary>

    public void AddIngredient(GameObject ingrediente)
    {
        if (ingredientCount >= maxIngredients) return;
        ingredientCount++;
        
        int eggLayerMask = LayerMask.NameToLayer("Egg");
        int milkLayerMask = LayerMask.NameToLayer("Milk");
        int flourLayerMask = LayerMask.NameToLayer("Flour");
        if (ingrediente.layer == eggLayerMask)
        {
            eggSound.Play();
        }
        if (ingrediente.layer == milkLayerMask)
        {
            milkSound.Play();
        }
        if (ingrediente.layer == flourLayerMask)
        {
            flourSound.Play();
        }
        doughSound.Play();
        // Si aún no hay masa, la instanciamos
        if (masaInstance == null)
        {
            masaInstance = Instantiate(
                masaPrefab,
                spawnPoint.position,
                spawnPoint.rotation,
                spawnPoint
            );
            masaInstance.transform.localScale = initialScale;
        }
        else
        {
            // Si ya existe, ampliamos su escala
            masaInstance.transform.localScale += scaleStep;
        }
    }
}
