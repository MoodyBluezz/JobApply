using System;
using UnityEngine;

public class ModalWindow : MonoBehaviour
{
    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private GameObject _uiModalWindow;
    
    private void OnEnable()
    {
        _objectPool.OnFieldsFilled += DisableModalWindow;
    }

    private void OnDisable()
    {
        _objectPool.OnFieldsFilled -= DisableModalWindow;
    }

    private void DisableModalWindow()
    {
        _uiModalWindow.SetActive(false);
    }
}
