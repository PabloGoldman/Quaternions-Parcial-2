using CustomMath;
using EjerciciosAlgebra;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respuestas : MonoBehaviour
{
    Quaternion quat;
    enum exercise { uno, dos, tres };

    [SerializeField] exercise ex;

    public Color vectorColor = Color.red;

    public Vec3 a;

    public float angle;

    Vec3 vectorA = new Vec3(10, 0, 0);
    Vec3 vectorB = new Vec3(10, 10, 0);
    Vec3 vectorC = new Vec3(20, 10, 0);
    Vec3 vectorD = new Vec3(20, 20, 0);

    // Start is called before the first frame update
    void Start()
    {
        VectorDebugger.EnableCoordinates();
        VectorDebugger.EnableEditorView();

        VectorDebugger.AddVector(Vector3.zero, vectorA, Color.green, nameof(vectorA));
        VectorDebugger.AddVector(vectorA, vectorB, Color.green, nameof(vectorB));
        VectorDebugger.AddVector(vectorB, vectorC, Color.blue, nameof(vectorC));
        VectorDebugger.AddVector(vectorC, vectorD, Color.cyan, nameof(vectorD));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (ex)
        {
            case exercise.uno:
                DisplayVectors(nameof(vectorA));
                vectorA = Quat.Euler(new Vec3(0, angle, 0)) * vectorA;
                VectorDebugger.UpdatePosition(nameof(vectorA), vectorA);
                break;
            case exercise.dos:
                DisplayVectors(nameof(vectorA));
                DisplayVectors(nameof(vectorB));
                DisplayVectors(nameof(vectorC));

                vectorA = Quat.Euler(new Vec3(0, angle, 0)) * vectorA;
                vectorB = Quat.Euler(new Vec3(0, angle, 0)) * vectorB;
                vectorC = Quat.Euler(new Vec3(0, angle, 0)) * vectorC;

                VectorDebugger.UpdatePosition(nameof(vectorA), vectorA);
                VectorDebugger.UpdatePosition(nameof(vectorB), vectorA, vectorB);
                VectorDebugger.UpdatePosition(nameof(vectorC), vectorB, vectorC);
                break;
            case exercise.tres:
                DisplayVectors(nameof(vectorA));
                DisplayVectors(nameof(vectorB));
                DisplayVectors(nameof(vectorC));
                DisplayVectors(nameof(vectorD));

                vectorA = Quat.Euler(new Vec3(angle, angle, 0)) * vectorA;
                vectorC = Quat.Euler(new Vec3(-angle, -angle, 0)) * vectorC;

                VectorDebugger.UpdatePosition(nameof(vectorA), vectorA);
                VectorDebugger.UpdatePosition(nameof(vectorB), vectorA, vectorB);
                VectorDebugger.UpdatePosition(nameof(vectorC), vectorB, vectorC);
                VectorDebugger.UpdatePosition(nameof(vectorD), vectorC, vectorD);
                break;
            default:
                break;
        }
    }

    private void DisplayVectors(string key)
    {
        VectorDebugger.TurnOnVector(key);
        VectorDebugger.EnableEditorView(key);
    }
}

