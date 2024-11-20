using UnityEngine;
using Cinemachine;

public class CameraSensitivityController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualAimCamera;
    public CinemachineVirtualCamera virtualTpsCamera;
    private CinemachinePOV povTpsComponent;
    private CinemachinePOV povAimComponent;
    private const string SensitivityPrefKey = "Sensitivity";

    void Start()
    {
        UpdateSensitivity();
    }

    public void UpdateSensitivity()
    {
        float sensitivity = PlayerPrefs.GetFloat(SensitivityPrefKey, 3f);

        povAimComponent = virtualAimCamera.GetCinemachineComponent<CinemachinePOV>();
        povTpsComponent = virtualTpsCamera.GetCinemachineComponent<CinemachinePOV>();

        if (povAimComponent != null && povTpsComponent != null)
        {
            povAimComponent.m_HorizontalAxis.m_MaxSpeed = sensitivity;
            povAimComponent.m_VerticalAxis.m_MaxSpeed = sensitivity;
            povTpsComponent.m_HorizontalAxis.m_MaxSpeed = sensitivity;
            povTpsComponent.m_VerticalAxis.m_MaxSpeed = sensitivity;
        }
    }
}
