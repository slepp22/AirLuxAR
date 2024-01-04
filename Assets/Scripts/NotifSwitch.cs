using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NotifSwitch : MonoBehaviour
{
    private DataManager _dataManager;
    public GameObject notifBanner;
    public GameObject vent10;
    public GameObject mask;
    public GameObject vent5;

    public void Start()
    {
        _dataManager = DataManager.Instance;
    }

    public void onClick()
    {
        switch (_dataManager.currentRoomConfig.ToString())
        {
            case "0000":
            case "0100":
            case "0101":
                notifBanner.SetActive(!notifBanner.activeInHierarchy);
                vent10.SetActive(!vent10.activeInHierarchy);
                break;
            case "0001":
            case "1000":
                notifBanner.SetActive(!notifBanner.activeInHierarchy);
                mask.SetActive(!mask.activeInHierarchy);
                break;
            case "1100":
                notifBanner.SetActive(!notifBanner.activeInHierarchy);
                vent5.SetActive(!vent5.activeInHierarchy);
                break;
        }
    }
}
