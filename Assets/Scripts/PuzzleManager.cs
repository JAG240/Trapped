using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Dinner Party")]
    [SerializeField] private List<DinnerPartyPlaceSet> puzzleSolution = new List<DinnerPartyPlaceSet>();
    [SerializeField] private PuzzleButton dinnerPartyButton;
    [SerializeField] private KeyBox dinnerPartyBox;

    private void Start()
    {
        dinnerPartyButton.submit += SubmitDinnerParty;
    }

    private void SubmitDinnerParty()
    {

        foreach(DinnerPartyPlaceSet set in puzzleSolution)
        {
            if(set.spot.placedObject == null || !set.spot.placedObject.name.Contains(set.nameCheck))
            {
                dinnerPartyButton.PlayAudio(false);
                dinnerPartyButton.ReleaseButton();
                return;
            }
        }

        dinnerPartyButton.PlayAudio(true);
        dinnerPartyBox.Unlock();
    }
}

[Serializable]
public class DinnerPartyPlaceSet
{
    [field: SerializeField] public PlaceableArea spot { get; private set; }
    [field: SerializeField] public string nameCheck { get; private set; }
}