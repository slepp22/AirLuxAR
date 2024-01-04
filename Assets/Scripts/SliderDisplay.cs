using UnityEngine;
using UnityEngine.UI;

public class SliderDisplay : MonoBehaviour
{
    private DataManager _dataManager;
    public Slider slider;
    public string unit;
    public Text text;

    public void Start()
    {
        _dataManager = DataManager.Instance;
    }

    public void OnSliderMoved()
    {
        text.text = slider.value + " " + unit;

        if (unit == "m2")
            _dataManager.UpdateRoomSlider(slider.value.ToString());
        else
            _dataManager.UpdatePeopleSlider(slider.value.ToString());
    }
}
