using System;
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
    private const int lineNumberAdd = 10;
    private const float directionScale = 0.5f;

    //Constantes que definem o tamanho do butao de handle
    private const float handleSize = 0.04f;
    private const float pickSize = .06f;
    //Para nao colocar nenhum handle selected by default
    private int selectedIndex = -1;

    private static Color[] modeColors = {
     Color.white,
     Color.yellow,
     Color.cyan
     };
    #endregion Fields

    #region Properties

    #endregion Properties

    #region Constructor

    #endregion Constructor

    #region Methods

    public override void OnInspectorGUI()
    {
        //Para ter um formato normal no inspector
        //DrawDefaultInspector();

        spline = target as BezierSpline;//Para que objecto vai ser costume
        EditorGUI.BeginChangeCheck();        //Novo value que vem do GUI
        bool inLoop = EditorGUILayout.Toggle("Loop", spline.InLoop);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            //Atribui o novo loop à spline
            spline.InLoop = inLoop;
        }
        if (selectedIndex >= 0 && selectedIndex < spline.PointsCount)
            DrawSelectedPointInspector();

        if (GUILayout.Button("Add Curve"))//se butao no inspector  for pressionado chama o metodo para adicionar uma curva
        {
            Undo.RecordObject(spline, "Add curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);//Helps in the undo process
        }
        if (GUILayout.Button("Update Objects"))//se butao no inspector  for pressionado chama o metodo para Update dos objectos
        {
            Undo.RecordObject(spline, "Update");
            spline.UpdatePointsNaSpline();
            EditorUtility.SetDirty(spline);//Helps in the undo process
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
        //for (int j = 0; j < spline.PointsCount-1; j+=3)
        //{
        //    Vector3 p0 = ShowPoint(j+0);
        //    Vector3 p1 = ShowPoint(j+1);
        //    Vector3 p2 = ShowPoint(j+2);
        //    Vector3 p3 = ShowPoint(j+3);


        //    Handles.color = Color.gray;
        //    Handles.DrawLine(p0, p1);
        //    //Handles.DrawLine(p1, p2);
        //    Handles.DrawLine(p2, p3);

        //    Vector3 lineStart = spline.GetPointInSplineCubic(j);
        //    Handles.color = Color.green;
        //    Handles.DrawLine(lineStart, lineStart + spline.GetDirection(j));

        //    //Numero de linhas que são calculadas (curveSteps)
        //    for (int i = 1; i <= curveSteps; i++)
        //    {
        //        // Ponto na curva a cada step da linha
        //        Vector3 lineEnd = spline.GetPointInSplineCubic(i / (float)curveSteps);
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
        for (int i = 1; i < spline.PointsCount; i += 3)//Go through all the splines (a cada 3 pontos)
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


    private void DrawSelectedPointInspector()
    {
        //Label no Gui
        GUILayout.Label("Selected Point");
        //Waiting for a change
        EditorGUI.BeginChangeCheck();
        //Ir buscar a posição do ponto selecionado
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetterPoint(selectedIndex));
        Debug.Log(selectedIndex);
        if (EditorGUI.EndChangeCheck())// Quando o listener termina 
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            //Definir o ponto com a nova modificaço
            spline.SetPoint(selectedIndex, point);
        }
        //Permite a modificação do modo do ponto
        EditorGUI.BeginChangeCheck();
        //Valor do modo selecionado no Layout
        CONTROLPOINTSMODE mode = (CONTROLPOINTSMODE)EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        //Transforma um ponto de local para world
        Vector3 point = handlePosition.TransformPoint(spline.GetterPoint(index));
        //Para obter sempre o mesmo tamanho para as handles (independente do tamanho d ecra)
        float size = HandleUtility.GetHandleSize(point);
        //Caso o ponto seja o inicial o tamanho será o dobro para puder vizualizar o inicio da spline
        if (index == 0) { size *= 1.5f; }
        //Modifica a cor do handle de acordo com o modo do control point selecionado
        Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
        //Verifica primeiro qual é o butão selecionado
        if (Handles.Button(point, handleRotation, handleSize * size, pickSize * size, Handles.DotCap))
        {
            //o indice 
            selectedIndex = index;
            //Sempre que exite um botao selecionado pedimos um repaint do inspector
            Repaint();
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
                spline.SetPoint(index, handlePosition.InverseTransformPoint(point));
            }
        }
        return point;
    }

    //Funcao que mostra todas as direções presentes na curva 
    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPointInSpline(0f);
        //Velocidade non primeiro ponto da spline
        //Handles.DrawLine(point, point + spline.GetDirectionCubic(0f) * directionScale);
        //O numero de linhas de velocidade a cada curva
        int steps = curveSteps * lineNumberAdd * spline.CurveCount;
        for (int i = 1; i < steps; i++)
        {
            point = spline.GetPointInSpline(i / (float)steps);
            //Direcções que parte do ponto que acaba a linha + a primeira derivada que da a velocidade a cada ponto
            Handles.DrawLine(point, point + spline.GetVelocity(i / (float)steps) * directionScale);
        }
    }

    //public void OnDrawGizmos()
    //{

    //    if (spline == null)
    //        return;

    //    Gizmos.color = Color.black;

    //    for (int i = 0; i < spline.PointsCount; i++)
    //    {
    //        Gizmos.DrawSphere(spline.GetPointInSpline(i/spline.PointsCount), 0.1f);
    //    }
        
    //}
    #endregion Methods

}
