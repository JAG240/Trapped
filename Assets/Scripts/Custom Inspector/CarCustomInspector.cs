using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Car))]
public class CarCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Car car = (Car)target;
        StreetBuilder streetBuilder = GameObject.Find("StreetBuilder").GetComponent<StreetBuilder>();
        if (GUILayout.Button("Stall Car"))
        {
            car.Stall();
            car.StartCoroutine(streetBuilder.StopMovement());
        }

        if (GUILayout.Button("Exit Car"))
        {
            car.ExitCar();
        }
    }
}