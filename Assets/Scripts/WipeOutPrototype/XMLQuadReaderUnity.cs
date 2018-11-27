using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class XMLQuadReaderUnity
{


    XmlDocument doc = new XmlDocument();

    public XMLQuadReaderUnity(string xml)
    {
        doc.LoadXml(xml);
    }

    public XMLQuadReaderUnity(XmlReader xlmPath)
    {
        doc.Load(xlmPath);
    }

    public void Inicializador(ref List<double>[] quads)
    {
        foreach (List<double> quad in quads)
            for (int i = 0; i < quads.Length; i++)
            {
                quads[i] = new List<double>();
            }
    }

    public List<double>[] XmlNodeConverter()
    {
        List<double>[] quads = new List<double>[4];
        Inicializador(ref quads);
        int i = 0;
        double value = 0;

        foreach (XmlNode node in doc.GetElementsByTagName("WiGateWay"))
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                double.TryParse(item.InnerXml, out value);
                quads[i].Add(value);
                i++;
            }
        }
        return quads;
    }

    public void SaveFilePath(string path)
    {
        doc.Save(path);
    }
   
    //public void GetNewValue()
    //{
    //    doc.NodeInserted += new XmlNodeChangedEventHandler(NodeInsertedHandler);
    //}

    public void NodeInsertedHandler(Object src, XmlNodeChangedEventArgs args)
    {
        //Console.WriteLine("Node " + args.Node.Name + " inserted!!");
    }

}
