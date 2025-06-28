using UnityEngine;

public class DoughShapeChanger : MonoBehaviour
{
    [Tooltip("Tag que identifica al rodillo")]
    public string rollerTag = "Rodillo";

    [Tooltip("Prefab final que reemplazará a esta masa")]
    public GameObject shapedPrefab;

    [Tooltip("Si quieres parentar el nuevo objeto a un transform concreto")]
    public Transform parentAfterReplacement;

    private bool hasShaped = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasShaped) return;    // sólo una vez
        if (!collision.collider.CompareTag(rollerTag)) return;

        hasShaped = true;

        // Instancia el prefab final en la misma posición y rotación
        GameObject newObj = Instantiate(
            shapedPrefab,
            transform.position,
            transform.rotation,
            parentAfterReplacement ? parentAfterReplacement : transform.parent
        );

        // Opcional: copia escala si la masa tenía un scale particular
        newObj.transform.localScale = transform.localScale;

        // Destruye la masa original
        Destroy(gameObject);
    }
}
