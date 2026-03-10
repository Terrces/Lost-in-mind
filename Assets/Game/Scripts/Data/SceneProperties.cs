using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTime
{
    private bool timerActive = false;
    private int _hours;
    private int _minutes;
    private int _seconds;
    public int Hours { get => _hours; set { _hours = timeUpdate(value, 13);} }
    public int Minutes { get => _minutes; set { if(updateNext(value,60)) Hours += value/60; _minutes = timeUpdate(value);} }
    public int Seconds { get => _seconds; set { if(updateNext(value,60)) Minutes += value/60; _seconds = timeUpdate(value);} }

    public IEnumerator StartCustomTime(float TimeUpdate = 1.0f)
    {
        timerActive = true;
        while (timerActive)
        {
            yield return new WaitForSeconds(TimeUpdate);
            Seconds += 1;
        }
    }

    private bool updateNext(int value, int secondValue) => value >= secondValue;
    private int timeUpdate(int value, int secondValue = 60)
    {
        var _value = value;
        if (_value < 0) _value = 0;
        return _value % secondValue;
    }


    private string GetTimeForText(int value)
    {
        if (value < 10)
        {
            return $"0{value}";
        }
        return value.ToString();
    }

    public string GetHourseForText() => GetTimeForText(_hours);
    public string GetMinutesForText() => GetTimeForText(_minutes);
    public string GetSecondsForText() => GetTimeForText(_seconds);
    public string GetFullTime() => $"{GetHourseForText()}:{GetMinutesForText()}:{GetSecondsForText()}";
    public string GetHMTime() => $"{GetHourseForText()}:{GetMinutesForText()}";
}

public class SceneProperties : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Inventory inventory;

    [SerializeField] private bool enableStartItems = true;
    [SerializeField] private List<GameObject> startItems;
    
    public LocalTime SceneTime = new LocalTime();

    void Start()
    {
        SceneTime.Hours = Random.Range(0,12);
        SceneTime.Minutes = Random.Range(0,60);
        SceneTime.Seconds = Random.Range(0,60);

        Debug.Log(SceneTime.GetFullTime());
        StartCoroutine(SceneTime.StartCustomTime(0.1f));

        
        if(startItems.Count != 0 && inventory != null && enableStartItems)
        {
            foreach (GameObject _obj in startItems)
            {
                inventory.AddItem(_obj, false);
            }
        }
    }

}
