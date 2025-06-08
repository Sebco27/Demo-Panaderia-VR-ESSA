using UnityEngine;

public class PlateMassController : MonoBehaviour
{
    [Header("Prefab de la masa y punto de spawn")]
    public GameObject masaPrefab;
    public Transform spawnPoint;

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
    public void AddIngredient()
    {
        if (ingredientCount >= maxIngredients) return;
        ingredientCount++;

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
