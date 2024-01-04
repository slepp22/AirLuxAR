using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public struct RoomConfig
{
    public bool IsBigRoom;
    public bool IsCrowded;
    public bool AreMasksOn;
    public int AirFlow;

    public RoomConfig(string inputParameters)
    {
        if (inputParameters.Length != 4)
        {
            Debug.Log("Bad Parameter, returning invalid room config");
            Debug.Log("This should do something else");
            IsBigRoom = false;
            IsCrowded = false;
            AreMasksOn = false;
            AirFlow = -1;
            return;
        }

        bool.TryParse(inputParameters[0].ToString().Equals("1").ToString(), out IsBigRoom);
        bool.TryParse(inputParameters[1].ToString().Equals("1").ToString(), out IsCrowded);
        bool.TryParse(inputParameters[2].ToString().Equals("1").ToString(), out AreMasksOn);
        int.TryParse(inputParameters[3].ToString(), out int parsedAirFlow);

        AirFlow = Mathf.Clamp(parsedAirFlow, 0, 2);
    }

    public string ToString()
    {
        string str = new string("");

        if (IsBigRoom) str += "1";
        else str += "0";
        
        if (IsCrowded) str += "1";
        else str += "0";
        
        if (AreMasksOn) str += "1";
        else str += "0";

        str += AirFlow switch
        {
            0 => "0",
            1 => "1",
            _ => "2"
        };

        return str;
    }
    public static bool operator ==(RoomConfig rc1, RoomConfig rc2)
    {
        return rc1.Equals(rc2);
    }

    public static bool operator !=(RoomConfig rc1, RoomConfig rc2)
    {
        return !(rc1 == rc2);
    }
}

public class DataManager : MonoBehaviour
{
    // Singleton Instance
    private static DataManager _instance;

    public static DataManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<DataManager>();

            if (_instance != null) return _instance;
            var dataManagerObject = new GameObject("DataManager");
            _instance = dataManagerObject.AddComponent<DataManager>();

            return _instance;
        }
    }
    
    
    // Private
    private Dictionary<string, int> _scenarios = new Dictionary<string, int>();
    private bool _isBigRoom = false;
    private bool _isCrowded = false;
    private bool _areMasksOn = true;
    private int _vents = 0;
    private int _riskPercentage;
    
    // Public, hidden
    [HideInInspector]
    public RoomConfig currentRoomConfig;
    
    // Public, visible
    public RoomVisualizer largeRoom;
    public RoomVisualizer smallRoom;
    public GameObject flowVisualizer;
    
    // UI elements
    public List<GameObject> coloredMeterList;
    public Text riskPercentage;
    public GameObject notificationDialog;
    
    /**
     *  Event methods
     */
    void Awake()
    {
        CreateAllScenarios();
    }

    private void Start()
    {
        UpdateRoomConfig(new RoomConfig("0010"));
    }

    private void OnApplicationQuit()
    {
        _instance = null;
    }

    public void UpdateRoomSlider(String value)
    {
        int intValue = int.Parse(value);

        bool newRoomBig = (intValue - 40 > 80 - intValue);
        
        if (_isBigRoom == newRoomBig)
        {
            return;
        }

        _isBigRoom = newRoomBig;
        
        RoomConfig roomConfig = currentRoomConfig;
        roomConfig.IsBigRoom = newRoomBig;

        Debug.Log(_isBigRoom ? "Wow that's a big room" : "This room is kinda...");

        UpdateRoomConfig(roomConfig);
        
        Debug.Log(currentRoomConfig.IsBigRoom ? "Wow that's *still* a big room" : "Well, nothing you can do...");
    }
    public void UpdatePeopleSlider(String value)
    {
        int intValue = int.Parse(value);

        bool newCrowded = (intValue - 1 > 20 - intValue);
        
        if (_isCrowded == newCrowded)
        {
            return;
        }

        _isCrowded = newCrowded;
        
        RoomConfig roomConfig = currentRoomConfig;
        roomConfig.IsCrowded = newCrowded;

        Debug.Log(_isCrowded ? "Wow that's a lot of people" : "Kinda lonely, in'it...?");

        UpdateRoomConfig(roomConfig);
        
        Debug.Log(currentRoomConfig.IsCrowded ? "Wow that's *still* crowded" : "Well, nothing you can do...");
    }
    
    public void ButtonClicked(String value)
    {
        bool.TryParse(value[0].ToString().Equals("1").ToString(), out var isMaskButton);
        int.TryParse(value[1].ToString(), out int parsedValue);

        var sentValue = Mathf.Clamp(parsedValue, 0, 2);

        if (isMaskButton)
        {
            bool newMaskOn = sentValue.Equals(1);
            
            if (newMaskOn == _areMasksOn) return;
            
            _areMasksOn = newMaskOn;

            RoomConfig roomConfig = currentRoomConfig;
            roomConfig.AreMasksOn = newMaskOn;
            Debug.Log(_areMasksOn ? "These are safe people!" : "merci kémion");
            
            UpdateRoomConfig(roomConfig);
            
            Debug.Log(currentRoomConfig.AreMasksOn ? "Still safe B)" : "*HONK *HONK");
        }
        else
        {
            if (_vents == sentValue) return;
            
            _vents = sentValue;

            RoomConfig roomConfig = currentRoomConfig;
            roomConfig.AirFlow = sentValue;
            Debug.Log("Vent: " + _vents);
            Debug.Log("0 - Nothing | 1 - Window | 2 - Vents");
            
            UpdateRoomConfig(roomConfig);
            
            Debug.Log("Room Config Vent: " + currentRoomConfig.AirFlow);
            Debug.Log("0 - Nothing | 1 - Window | 2 - Vents");
        }
    }
    /**
     * Class methods
     */
    public void UpdateRoomConfig(RoomConfig newConfig)
    {
        _scenarios.TryGetValue(newConfig.ToString(), out int newPercentage);

        if (newPercentage == 0)
        {
            Debug.Log("Invalid Room Configuration: " + newConfig.ToString());
            return;
        }

        currentRoomConfig = newConfig;
        _riskPercentage = newPercentage;
        riskPercentage.text = newPercentage.ToString() + " %";

        changeColor();

        flowVisualizer.SetActive(_vents == 2);

        //smallRoom.UpdateVisibility(currentRoomConfig);
        //largeRoom.UpdateVisibility(currentRoomConfig);

        /*
         * Todo
         * Change visibility of elements inside area target (airflow only?)
         * Change particle system's density
         * Change color of meter
         * Popup notification
         *
         * Summary
         *      4 functions
         *          showAirflow();
         *          adjustDensity();            // take the public riskPercentage variable
         *          changeMeterColor();         // take the public riskPercentage variable
         *          showNotification();         // inside a conditional if risk below threshold
         *
         *      4 public attributes
         *          gameObject airFlow;
         *          ParticleSystem particleSystem; // maybe not ?
         *
         *          List<gameObject> coloredMeterList;
         *          gameObject notificationDialog;
         */
    }

    private void changeColor()
    {
        foreach (var meter in coloredMeterList)
        {
            meter.SetActive(false);
        }

        switch (_riskPercentage)
        {
            case > 20:
                coloredMeterList[3].SetActive(true);
                notificationDialog.SetActive(true);
                break;
            case > 15:
                coloredMeterList[2].SetActive(true);
                notificationDialog.SetActive(true);
                break;
            case > 5:
                coloredMeterList[1].SetActive(true);
                notificationDialog.SetActive(false);
                break;
            default:
                coloredMeterList[0].SetActive(true);
                notificationDialog.SetActive(false);
                break;
        }
    }
    
    private void CreateAllScenarios()
    {
        _scenarios.Add(new RoomConfig("0010").ToString(), 4);
        _scenarios.Add(new RoomConfig("0011").ToString(), 2);
        _scenarios.Add(new RoomConfig("0012").ToString(), 1);
        _scenarios.Add(new RoomConfig("0000").ToString(), 37);
        _scenarios.Add(new RoomConfig("0001").ToString(), 16);
        _scenarios.Add(new RoomConfig("0002").ToString(), 6);
        _scenarios.Add(new RoomConfig("0110").ToString(), 4);
        _scenarios.Add(new RoomConfig("0111").ToString(), 2);
        _scenarios.Add(new RoomConfig("0112").ToString(), 1);
        _scenarios.Add(new RoomConfig("0100").ToString(), 37);
        _scenarios.Add(new RoomConfig("0101").ToString(), 16);
        _scenarios.Add(new RoomConfig("0102").ToString(), 6);
        _scenarios.Add(new RoomConfig("1010").ToString(), 2);
        _scenarios.Add(new RoomConfig("1011").ToString(), 1);
        _scenarios.Add(new RoomConfig("1012").ToString(), 1);
        _scenarios.Add(new RoomConfig("1000").ToString(), 21);
        _scenarios.Add(new RoomConfig("1001").ToString(), 8);
        _scenarios.Add(new RoomConfig("1002").ToString(), 3);
        _scenarios.Add(new RoomConfig("1110").ToString(), 2);
        _scenarios.Add(new RoomConfig("1111").ToString(), 1);
        _scenarios.Add(new RoomConfig("1112").ToString(), 1);
        _scenarios.Add(new RoomConfig("1100").ToString(), 21);
        _scenarios.Add(new RoomConfig("1101").ToString(), 8);
        _scenarios.Add(new RoomConfig("1102").ToString(), 3);
    }
}