using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomVisualizer : MonoBehaviour
{
    private DataManager _dataManagerInstance;
    private RoomConfig _currentConfig;

    public bool IsBigRoom;

    public GameObject largerCrowdRootObject;
    public GameObject ventilationRootObject;
    public GameObject openedWindowRootObject;
    public GameObject closedWindowRootObject;

    public ParticleSystem aerosolCloud;
    
    // Start is called before the first frame update
    void Awake()
    {
        _dataManagerInstance = DataManager.Instance;
        
        if (this.IsBigRoom)
        {
            if (_dataManagerInstance.largeRoom != null)
            {
                Debug.Log("Two instances of Big Room exist");
                return;   
            }

            _dataManagerInstance.largeRoom = this;
        }
        else
        {
            if (_dataManagerInstance.smallRoom != null)
            {
                Debug.Log("Two instances of Small Room exist");
                return;
            }

            _dataManagerInstance.smallRoom = this;
        }
    }

    public void UpdateVisibility(RoomConfig roomConfig)
    {
        if (roomConfig != _dataManagerInstance.currentRoomConfig) return;
        _currentConfig = roomConfig;

        this.gameObject.SetActive(_currentConfig.IsBigRoom == this.IsBigRoom);
        
        largerCrowdRootObject.SetActive(_currentConfig.IsCrowded);
        
        ventilationRootObject.SetActive(_currentConfig.AirFlow==2);
        
        openedWindowRootObject.SetActive(_currentConfig.AirFlow==1);
        
        closedWindowRootObject.SetActive(_currentConfig.AirFlow is 0 or 2);
    }
}
