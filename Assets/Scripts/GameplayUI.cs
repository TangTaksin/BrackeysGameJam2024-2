using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    Camera _cam;

    public Image targetImg;
    Interactable _targetTransform;

    private void Awake()
    {
        _cam = Camera.main;
        SetTargetTransform(null);
    }

    private void OnEnable()
    {
        Player.OnVariableChange += SetTargetTransform;
    }

    private void OnDisable()
    {
        Player.OnVariableChange += SetTargetTransform;
    }

    private void Update()
    {
        UpdateTargetImg();
    }

    private void SetTargetTransform(Interactable _in)
    {
        print(_in);
        _targetTransform = _in;
        
        targetImg.gameObject.SetActive(_targetTransform != null);
    }

    void UpdateTargetImg()
    {
        if (_targetTransform != null)
        targetImg.transform.position = _cam.WorldToScreenPoint(_targetTransform.transform.position);
    }
}
