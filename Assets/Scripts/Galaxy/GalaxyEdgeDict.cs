using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GalaxyEdgeDict : MonoBehaviour
{
    [SerializeField]
    private GalaxyMapEdge edgePrefab;

    [Header("Constellations in order")]
    [Space(15)]
    [SerializeField]
    private GameObject[] constellations;

    [Header("Intraconstellation edges")]
    [Space(4)]
    [SerializeField]
    private Vector2Int[] orionEdges;
    [Space(4)]
    [SerializeField]
    private Vector2Int[] libraEdges;
    [Space(15)]
    [SerializeField]
    private Vector2Int[] cetusEdges;

    [Header("Connections between constellations")]
    [SerializeField]
    private StringPair[] constellationConnectEdges;

    private Dictionary<GalaxyMapVertex, List<GalaxyMapVertex>> edges;

    private void Awake()
    {
        edges = new Dictionary<GalaxyMapVertex, List<GalaxyMapVertex>>();
    }

    private void Start()
    {
        List<Vector2Int[]> allEdges = new List<Vector2Int[]> { orionEdges, libraEdges, cetusEdges };
        List<GalaxyMapVertex[]> allVertices = new List<GalaxyMapVertex[]>();

        foreach (GameObject constellation in constellations)
        {
            allVertices.Add(constellation.GetComponentsInChildren<GalaxyMapVertex>());
        }
        
        for (int i = 0; i < constellations.Length; i++)
        {
            SetupConstellationEdges(allVertices[i], allEdges[i]);
        }

        ConnectConstellations(allVertices.SelectMany(i => i));
    }

    public IReadOnlyDictionary<GalaxyMapVertex, List<GalaxyMapVertex>> GetEdges()
    {
        return edges;
    }

    private void SetupConstellationEdges(GalaxyMapVertex[] vertices, Vector2Int[] edges)
    {
        foreach (Vector2Int edge in edges)
        {
            GalaxyMapVertex first = vertices[edge.x - 1];
            GalaxyMapVertex second = vertices[edge.y - 1];
            AddDoubleConnections(first, second);
        }
    }

    private void ConnectConstellations(IEnumerable<GalaxyMapVertex> vertices)
    {
        Dictionary<string, GalaxyMapVertex> nameDict = new Dictionary<string, GalaxyMapVertex>();
        foreach (GalaxyMapVertex vertex in vertices)
        {
            string name = vertex.GetSectorID();
            nameDict.Add(name, vertex);
        }

        foreach (StringPair pair in constellationConnectEdges)
        {
            AddDoubleConnections(nameDict[pair.start], nameDict[pair.end]);
        }
    }

    private void AddDoubleConnections(GalaxyMapVertex first, GalaxyMapVertex second)
    {
        AddConnection(first, second);
        AddConnection(second, first);
        GalaxyMapEdge edge = Instantiate(edgePrefab);
        edge.transform.SetParent(transform);
        edge.Setup(first, second);
    }

    private void AddConnection(GalaxyMapVertex start, GalaxyMapVertex end)
    {
        if (edges.TryGetValue(start, out List<GalaxyMapVertex> connections))
        {
            connections.Add(end);
        }
        else
        {
            edges[start] = new List<GalaxyMapVertex>() { end };
        }
    }


    [System.Serializable]
    private struct StringPair
    {
        public string start;
        public string end;
    }
}
