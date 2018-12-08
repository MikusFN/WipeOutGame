using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Serializable attribute lets you embed a class with sub properties in the inspector. (Assim é possivel vizualisar a classe (propriedades) no inspector)
[System.Serializable]
public class Path
{
    #region Flields
    //Force Unity to serialize a private field.
    //Makes a variable not show up in the inspector but be serialized
    [SerializeField, HideInInspector]
    private List<Vector2> points;
    #endregion

    #region Properties
    public int NumberPoints
    {
        //Numero de Pontos
        get { return points.Count; }
    }
    public int NumberSegments
    {
        //Cada segmento é constituido por 4 pontos, em que no primeiro temos 4 pontos criados e nos seguintes so criamos 3. ( numeroDePontos - numeroDePontosPorSegemnto /numeroDePontosAdicionadosCadaNovoSegmento + PrimeiroSegmento)
        get { return ((points.Count - 4) / 3) + 1; }
    }
    public Vector2 LastPosition
    { get { return points[NumberPoints - 1]; } }
    //Indexer of the points.
    public Vector2 this[int i]
    {
        get { return points[i]; }
    }
    #endregion

    #region Contructor
    public Path(Vector2 center)
    {
        //set de pontos inicias
        points = new List<Vector2>
        {
            center + (Vector2.up+Vector2.left)*0.5f,      //  # p1
            center + Vector2.left,                        //  |            center
                                                          //  #--------------#--------------# p3
            center + Vector2.right,                       //  p0                            |
            center + (Vector2.down+Vector2.right)*0.5f    //                                # p2
        };
    }
    #endregion 

    #region Methods
    public void AddSegment(Vector2 achorPosition)
    {
        // p4 => p3 + (p3 - p2) = 2 * p3 - p2 -> p3 deslocado na direcao do vetor p2p3 = p4  
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);                 //  # p1                          # p4                           # p5 
        // p5 => p4 + p6 / 2                                                                 //  |            center           |             center2          |
        points.Add((points[points.Count - 1] + achorPosition) * 0.5f);                       //  #------------#----------------# p3------------#--------------# p6 achorpoint
        //Novo Achorpoint                                                                    //  p0                            |                              
        points.Add(achorPosition);                                                           //                                # p2                            
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        //Cada Segmento é constituido por 4 pontos, em que o ultimo ponto do segmento anterior a i é o primeiro ponto do segmento i. (step de 3 em 3)
        return new Vector2[] { points[i * 3 + 0], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3] };
    }

    public void MovePoint(int i, Vector2 newPosition)
    {
        Vector2 deltaPoint = newPosition - points[i];
        points[i] = newPosition;

        if (i % 3 == 0) //Were moving an achorPoint
        {
            //Cheking bounds
            if (i + 1 < points.Count)           
                points[i + 1] += deltaPoint;    //Mover o ponto a seguir e anterior de acordo o movimento actual
            if (i - 1 > 0)                      
                points[i - 1] += deltaPoint;    //Mover o ponto a seguir e anterior de acordo o movimento actual
        }
        else//Were moving a control point
        {
            //Os anchor points aparecem de 3 em tres pontos, ou seja resto da divisao por 3 tem que ser 0.
            bool nextPointIsAnchor = (i + 1) % 3 == 0;
            //Se o proximo ponto é um anchor o proximo ponto de controlo esta dois à frente caso contrario esta dois atras. //Percorrer a lista de pontos
            int indexControlPoint = (nextPointIsAnchor) ? i + 2 : i - 2;                                                    //Percorrer a lista de pontos
            //Se o proximo ponto é um anchor o proximo ponto de anchor esta 1 à frente caso contrario esta 1 atras.         //Percorrer a lista de pontos
            int indexAnchorPoint = (nextPointIsAnchor) ? i + 1 : i - 1;                                                     //Percorrer a lista de pontos

            if (indexControlPoint >= 0 && indexControlPoint < points.Count) // Verificar se é uma edge
            {
                float distance = (points[indexAnchorPoint] - points[indexControlPoint]).magnitude; // aqui mede a distancia entre o proximo anchor point e control
                Vector2 direction = (points[indexAnchorPoint] - newPosition).normalized;           //Direcao do movimento para o novo ponto
                points[indexControlPoint] = points[indexAnchorPoint] + direction * distance;       //Permite manter a distancia entre os pontos
            }
        }
    }
    #endregion
}
