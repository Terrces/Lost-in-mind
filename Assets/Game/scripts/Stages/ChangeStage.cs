using System.Collections.Generic;
using UnityEngine;

public class ChangeStage : MonoBehaviour, Iinteractable
{
    public InteractionObjectTypes types {get;set;} = InteractionObjectTypes.Object;

    public PickUpPackageArea pickUpPackageArea;

    public List<InspectorStage> Stages;
    // public List<GameObject> stages;
    public int FirstStage = 0;
    private int currentStageNumber = 0;
    private GameObject currentStage;

    void Start()
    {
        changeStage(FirstStage);
    }

    public void Interact()
    {
        NextStage();
    }

    public void NextStage()
    {
        Stage stage = currentStage.GetComponent<Stage>(); 
        if(stage.PackagesDelivered != stage.PackagesNeedForComplite) return;

        if(currentStageNumber + 1 != Stages.Count)
        {
            changeStage(currentStageNumber += 1);
        }
    }

    private void changeStage(int number)
    {
        if(currentStage) Destroy(currentStage);
        currentStageNumber = number;
        currentStage = Instantiate(Stages[currentStageNumber].Stage);
        getStage(currentStage).PackagesNeedForComplite = Stages[number].PackagesNeedForComplite;
        if(pickUpPackageArea) pickUpPackageArea.stage = getStage(currentStage);
    }

    private Stage getStage(GameObject _gameObject)
    {
        return _gameObject.GetComponent<Stage>();
    }
}
