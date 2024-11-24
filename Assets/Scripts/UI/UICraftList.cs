using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class UICraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemDataEquipment> craftEquipments;

    void Start() {
        transform.parent.GetChild(0).GetComponent<UICraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }


    public void SetupCraftList() {

        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipments.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UICraftSlot>().SetupCraftSlot(craftEquipments[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        SetupCraftList();
    }

    public void SetupDefaultCraftWindow () {
        if (craftEquipments[0] != null)
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipments[0]);
    }

}
