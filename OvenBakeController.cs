using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class OvenBakeController : MonoBehaviour
{
    [Header("Socket y puerta")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    public Transform door;

    [Header("Rotaciones de la puerta")]
    public Vector3 openEuler = Vector3.zero;
    public Vector3 closedEuler = new Vector3(0, -90, 0);

    [Header("Tiempos")]
    public float bakeTime = 5f;
    public float doorSpeed = 180f;

    [Header("Materiales")]
    [Tooltip("Material final")]
    public Material finalMaterial;

    public AudioSource ovenSound;

    // Internos:
    GameObject currentItem;
    Material originalMaterial;
    Renderer itemRenderer;

    private void OnEnable()
    {
        socketInteractor.selectEntered.AddListener(OnItemInserted);
    }

    private void OnDisable()
    {
        socketInteractor.selectEntered.RemoveListener(OnItemInserted);
    }

    private void OnItemInserted(SelectEnterEventArgs args)
    {
        // Guardamos el item y su renderer
        currentItem   = args.interactableObject.transform.gameObject;
        itemRenderer  = currentItem.GetComponentInChildren<Renderer>();
        originalMaterial = itemRenderer.material;

        // Iniciamos la secuencia
        StartCoroutine(BakeSequence());
    }

    private IEnumerator BakeSequence()
    {
        // 1) Cerramos la puerta
        yield return StartCoroutine(RotateDoor(openEuler, closedEuler));

        // 2) Tiempo de horneado
        yield return new WaitForSeconds(bakeTime/2f);
        
        ovenSound.Play();

        // 3) Asignamos material final (opcional)
        if (itemRenderer != null && finalMaterial != null)
            itemRenderer.material = finalMaterial;

        // 4) Tiempo de horneado
        yield return new WaitForSeconds(bakeTime/2f);

        // 5) Abrimos la puerta
        yield return StartCoroutine(RotateDoor(closedEuler, openEuler));
    }

    private IEnumerator RotateDoor(Vector3 fromEuler, Vector3 toEuler)
    {
        float angle   = Vector3.Angle(fromEuler, toEuler);
        float duration= angle / doorSpeed;
        float elapsed = 0f;
        door.localEulerAngles = fromEuler;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            door.localEulerAngles = Vector3.LerpUnclamped(fromEuler, toEuler, t);
            yield return null;
        }
        door.localEulerAngles = toEuler;
    }
}
