using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class OvenBakeController : MonoBehaviour
{
    [Header("Socket y puerta")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    public Transform door;

    [Header("Rotaciones de la puerta")]
    [Tooltip("Rotación local con la puerta abierta (por defecto)")]
    public Vector3 openEuler = new Vector3(-89, 0, 0);
    [Tooltip("Rotación local con la puerta cerrada")]
    public Vector3 closedEuler = new Vector3(-89, -90, 0);

    [Header("Tiempos")]
    [Tooltip("Cuánto tarda en hornearse (s)")]
    public float bakeTime = 5f;
    [Tooltip("Velocidad de giro de la puerta (grados/s)")]
    public float doorSpeed = 180f;

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
        // El item ya ha sido "snapeado" al Attach Transform del socket
        // Arrancamos la secuencia de horneado
        StartCoroutine(BakeSequence());
    }

    private IEnumerator BakeSequence()
    {
        // 1) Cerramos la puerta
        yield return StartCoroutine(RotateDoor(openEuler, closedEuler));

        // 2) Esperamos el tiempo de horneado
        yield return new WaitForSeconds(bakeTime);

        // 3) Abrimos la puerta
        yield return StartCoroutine(RotateDoor(closedEuler, openEuler));
    }

    private IEnumerator RotateDoor(Vector3 fromEuler, Vector3 toEuler)
    {
        float elapsed = 0f;
        float duration = Vector3.Angle(fromEuler, toEuler) / doorSpeed;
        // Asegúrate de partir de la rotación “from”
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
