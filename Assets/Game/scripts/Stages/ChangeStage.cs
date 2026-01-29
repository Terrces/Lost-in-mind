using System.Collections.Generic;
using UnityEngine;

public class ChangeStage : MonoBehaviour, Iinteractable
{
    public InteractionObjectTypes types {get;set;} = InteractionObjectTypes.Object;

    public PickUpPackageArea pickUpPackageArea;

    public List<GameObject> stages;
    public int FirstStage = 0;
    private int currentStageNumber = 0;
    private GameObject currentStage;

    void Start()
    {
        currentStageNumber = FirstStage;
        currentStage = Instantiate(stages[FirstStage]);
        if(pickUpPackageArea) pickUpPackageArea.stage = getStage(currentStage);
    }

    public void Interact()
    {
        NextStage();
    }

    public void NextStage()
    {

        if(currentStageNumber + 1 != stages.Count)
        {
            changeStage(currentStageNumber += 1);
        }
    }

    private void changeStage(int number)
    {
        Destroy(currentStage);
        currentStageNumber = number;
        currentStage = Instantiate(stages[currentStageNumber]);
        if(pickUpPackageArea) pickUpPackageArea.stage = getStage(currentStage);
    }

    private Stage getStage(GameObject _gameObject)
    {
        return _gameObject.GetComponent<Stage>();
    }

    void OnValidate()
    {
        
    }
}
