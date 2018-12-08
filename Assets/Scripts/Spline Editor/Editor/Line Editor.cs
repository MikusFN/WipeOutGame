using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Line))]// Para ter um ediitor contum sempre que usarmos um objecto do tipo Line
public class LineEditor : Editor
{


    //Permite desenhar cenas no scene editor
    private void OnSceneGUI()
    {
        Line line = target as Line; // Variavel propria do OnSceneGUI method que permite definir qual o objecto a ser tratado por este metodo como target


        Transform handleTransform = line.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? //Merdas do unity para ter local e global rotation de acordo...
            handleTransform.rotation : Quaternion.identity;




        Vector3 p0LS = handleTransform.TransformPoint(line.P0); // transformar os pontos para local space para os handles os puderem tranformar.
        Vector3 p1LS = handleTransform.TransformPoint(line.P1);

        Handles.color = Color.green;
        //Handles.DrawLine(line.P0, line.P1);
        Handles.DrawLine(p0LS, p1LS);


        //Para ser possivel controlar as rotações e transformacoes de posicao a cada ponto da linha
        UnityCheckRotationChanges(ref p0LS, handleTransform, handleRotation, line, 0);
        UnityCheckRotationChanges(ref p1LS, handleTransform, handleRotation, line, 1);

        //Handles.DoPositionHandle(p0LS, handleRotation);
        //Handles.DoPositionHandle(p1LS, handleRotation);
    }


    private void UnityCheckRotationChanges(ref Vector3 p, Transform handleTransform, Quaternion handleRotation, Line line, int point)
    {
        EditorGUI.BeginChangeCheck();
        p = Handles.DoPositionHandle(p, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Object"); //Record of actions
            EditorUtility.SetDirty(line); // Inform Unty that this object as actions that need to be saved
            if (point == 0)
                line.P0 = handleTransform.InverseTransformPoint(p);
            else
                line.P1 = handleTransform.InverseTransformPoint(p);

        }
    }

}
