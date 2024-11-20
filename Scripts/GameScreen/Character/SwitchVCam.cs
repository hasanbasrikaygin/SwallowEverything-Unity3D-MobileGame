using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int priortyBoostAmount = 10;
    [SerializeField] private CinemachineVirtualCamera deathCamera; // �l�m kameras�
    [SerializeField] private Vector3 deathCameraOffset = new Vector3(-3, 50, -5); // Edit�rden ayarlanabilir offset
    [SerializeField] PlayerController playerController;
    public static int totalPriorty;
    private bool isAiming = false;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
        aimCanvas.enabled = false;
    }

    private void OnEnable()
    {
        aimAction.performed += _ => ToggleAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => ToggleAim();
    }

    private void ToggleAim()
    {
        isAiming = !isAiming;
        totalPriorty += 10;
        if (isAiming)
        {
            virtualCamera.Priority += priortyBoostAmount;
            aimCanvas.enabled = true;
            thirdPersonCanvas.enabled = false;
            playerController.offset = 25;
        }
        else
        {
            virtualCamera.Priority -= priortyBoostAmount;
            aimCanvas.enabled = false;
            thirdPersonCanvas.enabled = true;
            playerController.offset = 10;
        }
    }

    public void TriggerDeathCamera(Vector3 deathPosition)
    {
        // �l�m kameras�n� etkinle�tir ve sabit bir pozisyona ayarla
        deathCamera.Priority = totalPriorty + priortyBoostAmount + 1; // En y�ksek �ncelik ver

        // Kamera bile�enlerini devre d��� b�rak
        var pov = deathCamera.GetCinemachineComponent<CinemachinePOV>();
        if (pov != null)
        {
            pov.m_HorizontalAxis.m_MaxSpeed = 0;
            pov.m_VerticalAxis.m_MaxSpeed = 0;
        }

        // Kameran�n pozisyonunu ve y�n�n� ayarla
        Vector3 newPosition = deathPosition + deathCameraOffset;
        deathCamera.transform.position = newPosition;
        deathCamera.transform.LookAt(deathPosition);
    }
}
