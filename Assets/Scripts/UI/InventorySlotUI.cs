﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    private int _quantity;
    public int quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            quantityText.text = _quantity.ToString();
            if (value > 0) iconImage.gameObject.SetActive(true);
            else iconImage.gameObject.SetActive(false);
        }
    }
    public int itemIndex;
    private Texture2D _icon;
    public Texture2D icon
    {
        get => _icon;
        set
        {
            _icon = value;
            iconImage.texture = _icon;
        }
    }
    [SerializeField] private ItemMoveIcon display;
    [SerializeField] RawImage iconImage;
    [SerializeField] TextMeshProUGUI quantityText;
    private InventoryInteractionHandler iih => InventoryInteractionHandler.ins;
    private Inventory inventory => Inventory.ins;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (iih.isItemMoving)
        {
            var redundant = AddQuantity(iih.movingCount, iih.movingIcon);
            inventory.Move(iih.sourceItemIndex, iih.sourceItemCount, itemIndex, quantity);
            iih.ChangeMoveIconQuantity(redundant);
            iih.ChangeSourceItem(this);
        }
        else
        {
            iih.SetMovingItem(this);
        }
    }

    private int AddQuantity(int quantity, Texture2D icon)
    {
        this.quantity += quantity;
        this.icon = icon;
        int redundant = 0;
        if (this.quantity > inventory.maxInventorySlot)
        {
            redundant = this.quantity - inventory.maxInventorySlot;
            this.quantity = inventory.maxInventorySlot;
        }
        return redundant;
    }

}