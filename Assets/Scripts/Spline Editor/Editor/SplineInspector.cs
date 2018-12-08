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

    //Constantes que definem as linhas em cada curva e o tamanho das linhas de velocidade
    private const int curveSteps = 10;
    private const float directionScale = 2f;

    //Constantes que definem o tamanho do butao de handle
    private const float handleSize = 0.04f;
    private const float pickSize = .06f;
    //Para nao colocar nenhum handle selected by default
    private int selectedIndex = -1;
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

        if (GUILayout.Button("Add Curve"))
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
        #region Old Version
        //A funcao Show point serve para tranformar o ponto em world space,
        //Verificar se houve modificações nos handle,
        //Caso haja atribui ao ponto em world space e por fim passa novamente para local space
        //Assim temos uma posiçao modificada de um ponto.
        //for (int j = 0; j < spline.points.Length-1; j+=3)
        //{
        //    Vector3 p0 = ShowPoint(j+0);
        //    Vector3 p1 = ShowPoint(j+1);
        //    Vector3 p2 = ShowPoint(j+2);
        //    Vector3 p3 = ShowPoint(j+3);


        //    Handles.color = Color.gray;
        //    Handles.DrawLine(p0, p1);
        //    //Handles.DrawLine(p1, p2);
        //    Handles.DrawLine(p2, p3);

        //    Vector3 lineStart = spline.GetPointCubic(j);
        //    Handles.color = Color.green;
        //    Handles.DrawLine(lineStart, lineStart + spline.GetDirection(j));

        //    //Numero de linhas que são calculadas (curveSteps)
        //    for (int i = 1; i <= curveSteps; i++)
        //    {
        //        // Ponto na curva a cada step da linha
        //        Vector3 lineEnd = spline.GetPointCubic(i / (float)curveSteps);
        //        Handles.color = Color.white;
        //        Handles.DrawLine(lineStart, lineEnd);
        //        //Handles.color = Color.green;
        //        //Direcções que parte do ponto que acaba a linha + a primeira derivada que da a velocidade a cada ponto.
        //        //Handles.DrawLine(lineEnd, lineEnd + spline.GetDirectionCubic(i / (float)curveSteps) * directionScale);
        //        // O ponto anterior de fim é o novo inicio
        //        lineStart = lineEnd;
        //    }
        //    Handles.color = Color.green;
        //    ShowDirections();
        #endregion
        //Ponto inicial
        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < spline.points.Length; i += 3)//Go through all the splines (a cada 3 pontos)
        {
            //Tres pontos que fazem uma spline
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            //Linhas que ligam os pontos de ocntrolo
            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            //Curve Bezier
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);

            //Ultimo ponto da anterior é o primeiro da proxima
            p0 = p3;
        }
        ShowDirections();
    }

    private Vector3 ShowPoint(int index)
    {
        //Transforma um ponto de local para world
        Vector3 point = handlePosition.TransformPoint(spline.Points[index]);
        //Para obter sempre o mesmo tamanho para as handles (independente do tamanho d ecra)
        float size = HandleUtility.GetHandleSize(point);
        //Verifica primeiro qual é o butão selecionado
        if (Handles.Button(point, handleRotation, handleSize * size, pickSize * size, Handles.DotCap))
        {
            selectedIndex = index;
        }
        //So pode ser alturado o que esta actualmente selecionado
        if (selectedIndex == index)
        {
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
        }
        return point;
    }

    //Funcao que mostra todas as direções presentes na curva 
    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        //Velocidade non primeiro ponto da spline
        Handles.DrawLine(point, point + spline.GetDirectionCubic(0f) * directionScale);
        //O numero de linhas de velocidade a cada curva
        int steps = curveSteps * 10 * spline.CurveCount;
        for (int i = 1; i < steps; i++)
        {
            point = spline.GetPoint(i / (float)steps);
            //Direcções que parte do ponto que acaba a linha + a primeira derivada que da a velocidade a cada ponto
            Handles.DrawLine(point, point + spline.GetDirectionCubic(i / (float)steps) * directionScale);
        }
    }
    #endregion Methods

}
