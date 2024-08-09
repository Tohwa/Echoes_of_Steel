using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PosterInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionStatus interactionStatus;
    [SerializeField] private string oneLiner;

    private GameObject m_player;
    private GameObject m_camera;
    [SerializeField] private GameObject m_holoObj;
    [SerializeField] private GameObject interactMessage;

    public float zoomDuration = 5.0f;
    private float m_cameraOriginalDist;

    private Vector3 m_originalCameraPos;
    private Quaternion m_originalCameraRota;
    [SerializeField] private Transform m_cameraTarget;

    public void Interact()
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        m_player = GameObject.FindGameObjectWithTag("Player");

        interactionStatus.AddInteractedObject(gameObject.name);
        DialogueManager.instance.PlayOneLiner(oneLiner);

        StartCoroutine(ObservePoster());
    }

    private IEnumerator ObservePoster()
    {
        float elapsedTime = 0;
        float rotationSpeedFactor = 0.15f;
        
        m_cameraOriginalDist = m_camera.GetComponent<CameraController>().currentDist;
        
        while (elapsedTime < zoomDuration)
        {
            m_camera.GetComponent<CameraController>().currentDist = Mathf.Lerp(m_cameraOriginalDist, m_camera.GetComponent<CameraController>().cameraMinDist, elapsedTime / zoomDuration * 3);
            elapsedTime += Time.deltaTime * 2;
            yield return null;
        }

        m_originalCameraRota = m_camera.transform.rotation;
        m_originalCameraPos = m_camera.transform.position;
        Vector3 targetPos = m_cameraTarget.position;

        m_player.SetActive(false);
        m_holoObj.SetActive(false);

        m_camera.GetComponent<CameraController>().enabled = false;

        elapsedTime = 0;

        while (elapsedTime < zoomDuration)
        {
            m_camera.transform.position = Vector3.Lerp(m_player.transform.position, targetPos, elapsedTime / zoomDuration);
            m_camera.transform.rotation = Quaternion.Lerp(m_camera.transform.rotation, m_cameraTarget.rotation, elapsedTime / zoomDuration * rotationSpeedFactor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(5);

        elapsedTime = 0;

        while (elapsedTime < zoomDuration)
        {
            m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, m_originalCameraPos, elapsedTime / zoomDuration);
            m_camera.transform.rotation = Quaternion.Lerp(m_camera.transform.rotation, m_originalCameraRota, elapsedTime / zoomDuration * rotationSpeedFactor);
            elapsedTime += Time.deltaTime;

            if (m_camera.transform.position == m_originalCameraPos && m_camera.transform.rotation == m_originalCameraRota)
            {
                m_player.SetActive(true);
                m_holoObj.SetActive(true);
                m_camera.GetComponent<CameraController>().enabled = true;
            }
            yield return null;
        }

        elapsedTime = 0;

        while (elapsedTime < zoomDuration)
        {
            m_camera.GetComponent<CameraController>().currentDist = Mathf.Lerp(m_camera.GetComponent<CameraController>().currentDist, m_cameraOriginalDist, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        interactMessage.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        interactMessage.gameObject.SetActive(false);
    }
}
