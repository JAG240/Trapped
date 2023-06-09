using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Dinner Party")]
    [SerializeField] private PlaceableArea spot1;
    [SerializeField] private PlaceableArea spot2;
    [SerializeField] private PlaceableArea spot3;
    [SerializeField] private PlaceableArea spot4;
    [SerializeField] private PuzzleButton dinnerPartyButton;
    [SerializeField] private KeyBox dinnerPartyBox;

    private void Start()
    {
        dinnerPartyButton.submit += SubmitDinnerParty;
    }

    private void SubmitDinnerParty()
    {
        dinnerPartyBox.Unlock();
        dinnerPartyButton.ReleaseButton();
    }
}
