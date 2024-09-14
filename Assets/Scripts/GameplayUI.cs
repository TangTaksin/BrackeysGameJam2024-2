using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    Camera _cam;

    public Image itemImg;
    public Image stamina_fill;
    public Image targetImg;
    Interactable _targetTransform;

    private void Awake()
    {
        _cam = Camera.main;
        SetTargetTransform(null);
    }

    private void OnEnable()
    {
        Player.OnInteractableChange += SetTargetTransform;
        Player.OnItemChange += UpdateItem;
        Player.OnStaminaChange += UpdateStamina;
    }

    private void OnDisable()
    {
        Player.OnInteractableChange -= SetTargetTransform;
        Player.OnItemChange -= UpdateItem;
        Player.OnStaminaChange -= UpdateStamina;
    }

    private void Update()
    {
        UpdateTargetImg();
    }

    private void SetTargetTransform(object _in)
    {
        _targetTransform = _in as Interactable;
        
        if (targetImg)
            targetImg.gameObject.SetActive(_targetTransform != null);
    }

    void UpdateTargetImg()
    {
        if (_targetTransform != null)
        targetImg.transform.position = _cam.WorldToScreenPoint(_targetTransform.transform.position);
    }

    void UpdateStamina(float _stamina, float _baseSta)
    {
        if (!stamina_fill)
            return;

        var stam_perc = _stamina / _baseSta;
        stamina_fill.fillAmount = stam_perc;
    }

    void UpdateItem(object _item)
    {
        if (!itemImg)
            return;

        var _itemData = _item as ItemData;
        itemImg.gameObject.SetActive(_itemData != null);

        if (_itemData)
        itemImg.sprite = _itemData.item_icon;
    }
}
