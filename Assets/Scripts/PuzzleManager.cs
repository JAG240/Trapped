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

    [Header("Lanterns")]
    [SerializeField] private PuzzleButton lanternsButton;
    [SerializeField] private KeyBox lanternsKeyBox;
    [SerializeField] private List<LanternsOrder> lanternsSolution = new List<LanternsOrder>();

    private void Start()
    {
        dinnerPartyButton.submit += SubmitDinnerParty;
        lanternsButton.submit += SubmitLanterns;
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

    private void SubmitLanterns()
    {
        foreach(LanternsOrder lantern in lanternsSolution)
        {
            if (lantern.handle.lowered)
                lantern.handle.Interact(null);


        }

        lanternsButton.PlayAudio(true);
        lanternsKeyBox.Unlock();
    }
}

[Serializable]
public class DinnerPartyPlaceSet
{
    [field: SerializeField] public PlaceableArea spot { get; private set; }
    [field: SerializeField] public string nameCheck { get; private set; }
}

[Serializable]
public class LanternsOrder
{
    [field: SerializeField] public Lantern lantern { get; private set; }
    [field: SerializeField] public bool solvedState { get; private set; }
    [field: SerializeField] public PuzzleHandle handle { get; private set; }
}