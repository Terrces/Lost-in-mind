using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class ChangeStage : Interactable
{

    public PickUpPackageArea pickUpPackageArea;

    public List<InspectorStage> Stages;
    public int FirstStage = 0;
    private int currentStageNumber = 0;
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
        getStage(currentStage).PackagesNeedForComplite = Stages[number].PackagesNeedForComplite;
        if(pickUpPackageArea) pickUpPackageArea.stage = getStage(currentStage);
        currentStage.GetComponent<Stage>().navMeshSurface = navMeshSurface;
    }

    private Stage getStage(GameObject _gameObject)
    {
        return _gameObject.GetComponent<Stage>();
    }
}
