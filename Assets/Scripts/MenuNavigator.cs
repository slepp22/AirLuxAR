using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigator : MonoBehaviour
{
    public GameObject splashScreen;
    public GameObject sizeMenu;
    public GameObject peopleMenu;
    public GameObject maskMenu;
    public GameObject ventMenu;
    public GameObject headerSpace;
    public GameObject riskSpace;
    public GameObject notifSpace;
    public GameObject buttonGroup;
    
    public void closeSplashScreen()
    {
        splashScreen.SetActive(false);
        sizeMenu.SetActive(true);
        headerSpace.SetActive(true);
        riskSpace.SetActive(true);
        buttonGroup.SetActive(true);
    }

    public void openSizeMenu()
    {
        sizeMenu.SetActive(true);
        peopleMenu.SetActive(false);
        maskMenu.SetActive(false);
        ventMenu.SetActive(false);
    }
    public void openPeopleMenu()
    {
        sizeMenu.SetActive(false);
        peopleMenu.SetActive(true);
        maskMenu.SetActive(false);
        ventMenu.SetActive(false);
    }
    public void openMaskMenu()
    {
        sizeMenu.SetActive(false);
        peopleMenu.SetActive(false);
        maskMenu.SetActive(true);
        ventMenu.SetActive(false);
    }
    public void openVentMenu()
    {
        sizeMenu.SetActive(false);
        peopleMenu.SetActive(false);
        maskMenu.SetActive(false);
        ventMenu.SetActive(true);
    }
}
