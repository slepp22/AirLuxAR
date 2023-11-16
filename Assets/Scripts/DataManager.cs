using System;
using UnityEngine;
using System.Collections.Generic;

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

        bool.TryParse(inputParameters[0].ToString(), out IsBigRoom);
        bool.TryParse(inputParameters[1].ToString(), out IsCrowded);
        bool.TryParse(inputParameters[2].ToString(), out AreMasksOn);
        int.TryParse(inputParameters[4].ToString(), out int parsedAirFlow);

        AirFlow = Mathf.Clamp(parsedAirFlow, 0, 2);
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
    private Dictionary<RoomConfig, int> _scenarios = new Dictionary<RoomConfig, int>();
    
    // Public, hidden
    [HideInInspector]
    public int riskPercentage;
    [HideInInspector]
    public RoomConfig currentRoomConfig;
    
    // Public, visible
    public RoomVisualizer largeRoom;
    public RoomVisualizer smallRoom;
    
    /**
     *  Event methods
     */
    void Awake()
    {
        CreateAllScenarios();
    }

    private void Start()
    {
        UpdateRoomConfig(new RoomConfig("0000"));
    }

    private void OnApplicationQuit()
    {
        _instance = null;
    }

    /**
     * Class methods
     */
    public void UpdateRoomConfig(RoomConfig newConfig)
    {
        _scenarios.TryGetValue(newConfig, out int newPercentage);

        if (newPercentage == 0)
        {
            Debug.Log("Invalid Room Configuration");
            return;
        }

        currentRoomConfig = newConfig;
        riskPercentage = newPercentage;
        
        
        smallRoom.UpdateVisibility(currentRoomConfig);
        largeRoom.UpdateVisibility(currentRoomConfig);
    }
    
    private void CreateAllScenarios()
    {
        _scenarios.Add(new RoomConfig("0010"), 4);
        _scenarios.Add(new RoomConfig("0011"), 2);
        _scenarios.Add(new RoomConfig("0012"), 1);
        _scenarios.Add(new RoomConfig("0000"), 37);
        _scenarios.Add(new RoomConfig("0001"), 16);
        _scenarios.Add(new RoomConfig("0002"), 6);
        _scenarios.Add(new RoomConfig("0110"), 4);
        _scenarios.Add(new RoomConfig("0111"), 2);
        _scenarios.Add(new RoomConfig("0112"), 1);
        _scenarios.Add(new RoomConfig("0100"), 37);
        _scenarios.Add(new RoomConfig("0101"), 16);
        _scenarios.Add(new RoomConfig("0102"), 6);
        _scenarios.Add(new RoomConfig("1010"), 2);
        _scenarios.Add(new RoomConfig("1011"), 1);
        _scenarios.Add(new RoomConfig("1012"), 1);
        _scenarios.Add(new RoomConfig("1000"), 21);
        _scenarios.Add(new RoomConfig("1001"), 8);
        _scenarios.Add(new RoomConfig("1002"), 3);
        _scenarios.Add(new RoomConfig("1110"), 2);
        _scenarios.Add(new RoomConfig("1111"), 1);
        _scenarios.Add(new RoomConfig("1112"), 1);
        _scenarios.Add(new RoomConfig("1100"), 21);
        _scenarios.Add(new RoomConfig("1101"), 8);
        _scenarios.Add(new RoomConfig("1102"), 3);
    }
}