//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(PathCreator))]
//public class PathEditor : Editor
//{

//    #region Fields
//    PathCreator pathCreator;
//    Path path;
//    #endregion Fields

//    #region Properties

//    #endregion Properties

//    #region Constructor

//    #endregion Constructor

//    #region Methods
//    void OnEnable()
//    {
//        pathCreator = (PathCreator)target;

//        if (pathCreator.Path == null)// Cria caminho caso este não exita
//        {
//            pathCreator.CreatePath();
//        }
//        path = pathCreator.Path;

//        //for (int i = 0; i < path.NumberPoints; i++)
//        //{
//        //    Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), (Vector3)path[i], Quaternion.identity);
//        //}
        
//    }
//    void OnSceneGUI()
//    {
//        Input();
//        Draw();
//    }
//    void Input()
//    {
//        //Vamos buscar os event que é actual
//        Event guiEvent = Event.current;
//        //Posicao do rato na Scene e convertemos para 2D
//        Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        
//        //Caso precisonemos o butão esquerdo do rato
//        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
//        {
//            Undo.RecordObject(pathCreator, "Add Segment");
//            path.AddSegment(mousePosition);// Adicionamos o segmento onde carregamos
//            //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), (Vector3)path[path.NumberPoints-1], Quaternion.identity);
//        }
//    }
//    void Draw()
//    {

//        for (int j = 0; j < path.NumberSegments; j++)
//        {
//            Handles.color = Color.yellow;
//            Vector2[] points = path.GetPointsInSegment(j);
//            Handles.DrawLine(points[1], points[0]);//Linhas que ligam o ponto de control ao ponto mais proximo que pertence à curva
//            Handles.DrawLine(points[2], points[3]);//Linhas que ligam o ponto de control ao ponto mais proximo que pertence à curva
//            // Bezier liga o primeiro ao ultimo (p0 a p3) de acordo com a tangent que começa no ponto de controlo 1 e acaba no ponto de controlo 2 (p1 e p2)
//            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.black, null, 2.0f);
//        }

//        //cor dos handles
//        Handles.color = Color.green;
//        for (int i = 0; i < path.NumberPoints; i++)
//        {
//            //Vai buscar a posicao do handle
//            Vector2 newPosition = Handles.FreeMoveHandle(path[i], Quaternion.identity, 0.1f, Vector2.zero, Handles.CylinderHandleCap);
//            if (path[i] != newPosition)// caso essa posicao seja nova
//            {
//                Undo.RecordObject(pathCreator, "Move Point");
//                path.MovePoint(i, newPosition);//move o ponto para essa nova posicao
//            }
//        }
//        //Undo.FlushUndoRecordObjects();
//    }
//    #endregion Methods
//}
