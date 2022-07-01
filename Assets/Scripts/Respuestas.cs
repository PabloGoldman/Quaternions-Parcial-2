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
                vectorA = Quat.Euler(new Vec3(0, angle, 0)) * vectorA;
                break;
            case exercise.dos:
                List<Vector3> newPositions2 = new List<Vector3>();
                for (int index = 0; index < VectorDebugger.GetVectorsPositions("2").Count; ++index)
                    newPositions2.Add(Quaternion.Euler(new Vector3(0.0f, this.angle, 0.0f)) * VectorDebugger.GetVectorsPositions("2")[index]);
                VectorDebugger.UpdatePositionsSecuence("2", newPositions2);
                break;
            case exercise.tres:
                break;
            default:
                break;
        }
    }
}
