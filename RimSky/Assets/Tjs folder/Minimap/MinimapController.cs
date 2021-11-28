using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public GameObject MiniMapCameraPrefab;
    public Camera _minimapCamera;
    public float _hight;

    private void Start()
    {
        GameObject minimapCamera = Instantiate(MiniMapCameraPrefab, GameManager.Instance.Player.transform.position + Vector3.up * _hight, transform.rotation, GameManager.Instance.Player.transform);
        _minimapCamera = minimapCamera.GetComponentInChildren<Camera>();
        _minimapCamera.orthographicSize = 100;
    }

}
