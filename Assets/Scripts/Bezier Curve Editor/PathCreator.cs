using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    #region Fields
    [HideInInspector]
     private Path path;
    #endregion Fields

    #region Properties
    public Path Path { get { return path; } }
    #endregion Properties

    #region Constructor

    #endregion Constructor

    #region Methods
    public void CreatePath()
    {
        //Caminho criado apartir da posicao deste objecto.
        path = new Path(this.transform.position);
    }
    #endregion Methods

}
