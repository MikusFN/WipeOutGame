using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
//using System.Threading.Tasks;


public class XMLReaderMain : MonoBehaviour
{

    //private string path = "C:\\Sensing Future Technologies\\wigateway.xml";

    public void Inicializador(ref List<double>[] quads)
    {
        foreach (List<double> quad in quads)
            for (int i = 0; i < quads.Length; i++)
            {
                quads[i] = new List<double>();
            }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //List<double>[] quads;
        List<double>[] quadsaux;
        //quads = new List<double>[4];
        quadsaux = new List<double>[4];
        Inicializador(ref quadsaux);


        //Thread.Sleep(100);-> Need to do a co routine to have the wait results that i want

        //    Task.Run(() =>// Para colocar a task em queue e quando começar nada mais pode correr enquanto esta Task nao acabar
        //    {
        //        XMLQuadReaderUnity quadReader = new XMLQuadReaderUnity(File.ReadAllText(path));
        //        lock (quadReader) // bloqueia este objecto até acabar o bloco de codigo (bloqueia em memoria e diz que este objecto nao esta disponivel)
        //        {
        //            //i++;
        //            quadReader.Inicializador(ref quads);

        //            quads = quadReader.XmlNodeConverter();
        //            for (int j = 0; j < 4; j++)
        //            {
        //                quadsaux[j].Add(quads[j][0]);
        //            }
        //            foreach (List<double> item in quadsaux)
        //            {
        //                foreach (double val in item)
        //                {
        //                   // Debug.Log((float)val);

        //                    //Console.WriteLine(val);
        //                    //quadsaux[j].Add(val);
        //                    //Console.WriteLine($"Times-> {i}");
        //                }
        //            }
        //        }
        //    });
    }
}
