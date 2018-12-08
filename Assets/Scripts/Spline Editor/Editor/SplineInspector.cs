using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class SplineInspector : Editor
{
    #region Fields
    private BezierSpline spline;
    private Transform handlePosition;
    private Quaternion handleRotation;

    private const int lineSteps = 100;
    private const float directionScale = 1.8f;
    #endregion Fields

    #region Properties

    #endregion Properties

    #region Constructor

    #endregion Constructor

    #region Methods

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        spline = target as BezierSpline;

        if(GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
    }

    private void OnSceneGUI()
    {
        spline = target as BezierSpline;
        handlePosition = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handlePosition.rotation : Quaternion.identity;

        //A funcao Show point serve para tranformar o ponto em world space,
        //Verificar se houve modificações nos handle,
        //Caso haja atribui ao ponto em world space e por fim passa novamente para local space
        //Assim temos uma posiçao modificada de um ponto.
        Vector3 p0 = ShowPoint(0);
        Vector3 p1 = ShowPoint(1);
        Vector3 p2 = ShowPoint(2);
        Vector3 p3 = ShowPoint(3);


        Handles.color = Color.gray;
        Handles.DrawLine(p0, p1);
        //Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);


        Vector3 lineStart = spline.GetPoint(0f);
        Handles.color = Color.green;
        Handles.DrawLine(lineStart, lineStart + spline.GetDirection(0f));

        //Numero de linhas que são calculadas (lineSteps)
        for (int i = 1; i <= lineSteps; i++)
        {
            // Ponto na curva a cada step da linha
            Vector3 lineEnd = spline.GetPointCubic(i / (float)lineSteps);
            Handles.color = Color.white;
            Handles.DrawLine(lineStart, lineEnd);
            //Handles.color = Color.green;
            //Direcções que parte do ponto que acaba a linha + a primeira derivada que da a velocidade a cada ponto.
            //Handles.DrawLine(lineEnd, lineEnd + spline.GetDirectionCubic(i / (float)lineSteps) * directionScale);
            // O ponto anterior de fim é o novo inicio
            lineStart = lineEnd;
        }
        Handles.color = Color.green;
        ShowDirections();
    }


    private Vector3 ShowPoint(int index)
    {
        if (index >= spline.Points.Length && index > 0)
            index = spline.Points.Length - 1;

        //Transforma um ponto de local para world
        Vector3 point = handlePosition.TransformPoint(spline.Points[index]);
        EditorGUI.BeginChangeCheck();

        //Modifica o ponto no seu world space de acordo com o handle
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            //Modifica o ponto no seu local space
            spline.Points[index] = handlePosition.InverseTransformPoint(point);
        }
        return point;
    }

    //Funcao que mostra todas as direções presentes na curva 
    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPointCubic(0f);
        Handles.DrawLine(point, point + spline.GetDirectionCubic(0f) * directionScale);

        for (int i = 1; i <= lineSteps; i++)
        {
            point = spline.GetPointCubic(i / (float)lineSteps);
            Handles.DrawLine(point, point + spline.GetDirectionCubic(i / (float)lineSteps) * directionScale);
        }
    }
    #endregion Methods

}
