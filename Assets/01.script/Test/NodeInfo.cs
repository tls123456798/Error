using System.Collections.Generic;
using UnityEngine;

public class NodeInfo
{
    public int x;
    public int y;
    public string nodeType;
    public List<NodeInfo> nextNodes = new List<NodeInfo>();
    public GameObject nodeObject; 
}
