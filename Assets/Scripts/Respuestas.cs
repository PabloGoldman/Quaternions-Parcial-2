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

        VectorDebugger.AddVector(new Vector3(10f, 0.0f, 0.0f), Color.red, "1");
        List<Vector3> positions1 = new List<Vector3>();
        positions1.Add(new Vector3(10f, 0.0f, 0.0f));
        positions1.Add(new Vector3(10f, 10f, 0.0f));
        positions1.Add(new Vector3(20f, 10f, 0.0f));
        VectorDebugger.AddVectorsSecuence(positions1, false, Color.blue, "2");
        List<Vector3> positions2 = new List<Vector3>();
        positions2.Add(new Vector3(10f, 0.0f, 0.0f));
        positions2.Add(new Vector3(10f, 10f, 0.0f));
        positions2.Add(new Vector3(20f, 10f, 0.0f));
        positions2.Add(new Vector3(20f, 20f, 0.0f));
        VectorDebugger.AddVectorsSecuence(positions2, false, Color.yellow, "3");
    }

    // Update is called once per frame
    void Update()
    {
        switch (ex)
        {
            case exercise.uno:
                ShowVector(nameof(vectorA));
                vectorA = Quat.Euler(new Vec3(0, angle, 0)) * vectorA;
                VectorDebugger.UpdatePosition(nameof(vectorA), vectorA);
                break;
            case exercise.dos:
                ShowVector(nameof(vectorA));
                ShowVector(nameof(vectorB));
                ShowVector(nameof(vectorC));

                vectorA = Quat.Euler(new Vec3(0, angle, 0)) * vectorA;
                vectorB = Quat.Euler(new Vec3(0, angle, 0)) * vectorB;
                vectorC = Quat.Euler(new Vec3(0, angle, 0)) * vectorC;

                VectorDebugger.UpdatePosition(nameof(vectorA), vectorA);
                VectorDebugger.UpdatePosition(nameof(vectorB), vectorA, vectorB);
                VectorDebugger.UpdatePosition(nameof(vectorC), vectorB, vectorC);
                break;
            case exercise.tres:
                break;
            default:
                break;
        }
    }

    private void ShowVector(string key)
    {
        VectorDebugger.TurnOnVector(key);
        VectorDebugger.EnableEditorView(key);
    }
}

