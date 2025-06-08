using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ChangeMasaColorAndSpin : MonoBehaviour
{
    [Header("Socket y material")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    public Material highlightMaterial;
    public bool materialCambiado = false;

    [Header("Delays y rotación")]
    [Tooltip("Retraso antes de empezar a girar (s)")]
    public float spinDelay = 5f;
    [Tooltip("Eje local de rotación")]
    public Vector3 rotationAxis = Vector3.up;
    [Tooltip("Velocidad de giro (grados/s)")]
    public float spinSpeed = 360f;
    [Tooltip("Duración del giro (s)")]
    public float spinDuration = 3f;

    private void OnEnable()
    {
        socketInteractor.selectEntered.AddListener(OnMasaSelected);
    }

    private void OnDisable()
    {
        socketInteractor.selectEntered.RemoveListener(OnMasaSelected);
    }

    private void OnMasaSelected(SelectEnterEventArgs args)
    {
        // 1) Cambia material
        var rend = args.interactableObject.transform.GetComponentInChildren<Renderer>();
        if (rend == null)
        {
            return;
        }

        // 2) Inicia la corutina con retardo
        StartCoroutine(DelayedSpinRoutine(args.interactableObject.transform, rend));
    }

    private IEnumerator DelayedSpinRoutine(Transform target, Renderer rend)
    {
        // Esperamos el tiempo de delay
        yield return new WaitForSeconds(spinDelay);
        socketInteractor.socketActive = false;

        // Rotamos durante spinDuration
        float elapsed = 0f;
        while (elapsed < spinDuration)
        {
            target.Rotate(rotationAxis.normalized * spinSpeed * Time.deltaTime,
                          Space.Self);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!materialCambiado)
        {
            rend.material = highlightMaterial;
            materialCambiado = true;
        }

        // Una vez terminado el giro, volvemos a activar el socket
        socketInteractor.socketActive = true;
    }
}
