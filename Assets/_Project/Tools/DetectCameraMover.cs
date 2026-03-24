using System.Linq;
using UnityEngine;

public class DetectCameraMover : MonoBehaviour
{
    void Start()
    {
        Debug.Log("DetectCameraMover: Starting camera diagnostics...");

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("DetectCameraMover: No Camera tagged 'MainCamera' found (Camera.main is null).");
            return;
        }

        GameObject mainCamGo = cam.gameObject;
        Debug.Log($"Main Camera GameObject: {mainCamGo.name}");
        Debug.Log($"Parent: {(mainCamGo.transform.parent != null ? mainCamGo.transform.parent.name : "(no parent)")}");

        var comps = mainCamGo.GetComponents<MonoBehaviour>();
        if (comps.Length == 0)
            Debug.Log("Main Camera: no MonoBehaviour scripts attached (only Camera/Transform). If using Cinemachine, check for Virtual Cameras in scene.");
        else
        {
            Debug.Log($"Main Camera has {comps.Length} MonoBehaviour components:");
            foreach (var c in comps)
            {
                if (c == null) continue;
                Debug.Log($" - {c.GetType().FullName} (on GameObject: {c.gameObject.name})");
            }
        }

        var cameraControllers = FindObjectsOfType<MonoBehaviour>().Where(m => m.GetType().Name == "CameraController").ToArray();
        Debug.Log($"Found {cameraControllers.Length} component(s) named 'CameraController' in the scene:");
        foreach (var cc in cameraControllers)
        {
            if (cc == null) continue;
            Debug.Log($" - {cc.GetType().FullName} on GameObject: {cc.gameObject.name}");
        }

        var allCameras = FindObjectsOfType<Camera>();
        Debug.Log($"Total Camera components in scene: {allCameras.Length}");
        foreach (var c in allCameras)
        {
            Debug.Log($" Camera: {c.gameObject.name} (tag={c.gameObject.tag}) parent={(c.transform.parent!=null?c.transform.parent.name:"(no parent)")}");
        }

        var player = GameObject.FindWithTag("Player");
        Debug.Log(player != null ? $"Player found: {player.name}" : "No GameObject with tag 'Player' found.");

        Debug.Log("DetectCameraMover: Diagnostics complete. Inspect Console for details.");
    }
}
