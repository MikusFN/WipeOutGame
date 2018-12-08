using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor
{
    #region Fields
    private BezierCurve curve;
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
    private void OnSceneGUI()
    {
        curve = target as BezierCurve;
        handlePosition = curve.transform;
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


        Vector3 lineStart = curve.GetPoint(0f);
        Handles.color = Color.green;
        Handles.DrawLine(lineStart, lineStart + curve.GetDirection(0f));

        //Numero de linhas que são calculadas (lineSteps)
        for (int i = 1; i <= lineSteps; i++)
        {
            // Ponto na curva a cada step da linha
            Vector3 lineEnd = curve.GetPointCubic(i / (float)lineSteps);
            Handles.color = Color.white;
            Handles.DrawLine(lineStart, lineEnd);
            //Handles.color = Color.green;
            //Direcções que parte do ponto que acaba a linha + a primeira derivada que da a velocidade a cada ponto.
            //Handles.DrawLine(lineEnd, lineEnd + curve.GetDirectionCubic(i / (float)lineSteps) * directionScale);
            // O ponto anterior de fim é o novo inicio
            lineStart = lineEnd;
        }
        Handles.color = Color.green;
        ShowDirections();
    }
    

    private Vector3 ShowPoint(int index)
    {
        if (index >= curve.Points.Length && index > 0)
            index = curve.Points.Length - 1;

        //Transforma um ponto de local para world
        Vector3 point = handlePosition.TransformPoint(curve.Points[index]);
        EditorGUI.BeginChangeCheck();

        //Modifica o ponto no seu world space de acordo com o handle
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            //Modifica o ponto no seu local space
            curve.Points[index] = handlePosition.InverseTransformPoint(point);
        }
        return point;
    }

    //Funcao que mostra todas as direções presentes na curva 
    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = curve.GetPointCubic(0f);
        Handles.DrawLine(point, point + curve.GetDirectionCubic(0f) * directionScale);

        for (int i = 1; i <= lineSteps; i++)
        {
            point = curve.GetPointCubic(i / (float)lineSteps);
            Handles.DrawLine(point, point + curve.GetDirectionCubic(i / (float)lineSteps) * directionScale);
        }
    }
    #endregion Methods
}
