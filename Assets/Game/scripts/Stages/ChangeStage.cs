using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class ChangeStage : Interactable
{

    public PickUpPackageArea pickUpPackageArea;

    public List<InspectorStage> Stages;
    public int FirstStage = 0;
    public int currentStageNumber {get; private set;}
    public GameObject currentStage {get; private set;}
    public NavMeshSurface navMeshSurface;

    void Awake() => Interacted += ObjectInteraction;
    void Start()
    {
        changeStage(FirstStage);
    }

    public void ObjectInteraction()
    {
        NextStage();
    }

    public void NextStage()
    {
        Stage stage = currentStage.GetComponent<Stage>(); 
        if((stage.PackagesDelivered != stage.PackagesNeedForComplite) || (currentStageNumber + 1 == Stages.Count)) return;

        changeStage(currentStageNumber += 1);
    }

    private void changeStage(int number)
    {
        if(currentStage) Destroy(currentStage);
        
        currentStageNumber = number;
        currentStage = Instantiate(Stages[currentStageNumber].Stage);
        
        Stage stage = getStage(currentStage);
        
        if(pickUpPackageArea) pickUpPackageArea.stage = stage;
        
        stage.PackagesNeedForComplite = Stages[number].PackagesNeedForComplite;
        stage.StageNumber = currentStageNumber;

        stage.navMeshSurface = navMeshSurface;
    }

    private Stage getStage(GameObject _gameObject)
    {
        return _gameObject.GetComponent<Stage>();
    }
}
