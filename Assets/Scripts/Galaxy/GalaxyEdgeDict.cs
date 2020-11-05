using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Reader;

public class GalaxyEdgeDict : MonoBehaviour
{
    [SerializeField]
    private GalaxyMapEdge edgePrefab;

    [Header("Constellations in order")]
    [Space(15)]
    [SerializeField]
    private GameObject[] constellations;

    [Header("Intraconstellation edges")]
    [SerializeField]
    private TextAsset[] sectorConnectTexts;

    [Header("Connections between constellations")]
    [SerializeField]
    private TextAsset constellationConnectText;

    [Header("Connections between constellations")]
    [SerializeField]
    private TextAsset[] subsectorConnectTexts;


    private Dictionary<GalaxyMapVertex, List<GalaxyMapVertex>> edges;

    private void Awake()
    {
        edges = new Dictionary<GalaxyMapVertex, List<GalaxyMapVertex>>();
    }

    private void Start()
    {
        List<List<StringPair>> sectorEdges = new List<List<StringPair>>();

        foreach (TextAsset sectorConnection in sectorConnectTexts)
        {
            sectorEdges.Add(Reader.ReadPairedFile(sectorConnection, "\t"));
        }

        List<GalaxyMapSector[]> allMainSectors = new List<GalaxyMapSector[]>();
        List<GalaxyMapSubsector[]> allSubsectors = new List<GalaxyMapSubsector[]>();
        List<string> names = new List<string>();

        GameObject constellationEdgeContainer = AddContainer("Intra-constellation edges", transform);
        foreach (GameObject constellation in constellations)
        {
            allMainSectors.Add(constellation.GetComponentsInChildren<GalaxyMapSector>());
            allSubsectors.Add(constellation.GetComponentsInChildren<GalaxyMapSubsector>());
            names.Add(constellation.name);
        }
        
        for (int i = 0; i < constellations.Length; i++)
        {
            SetupConstellationEdges(allMainSectors[i], sectorEdges[i], names[i], constellationEdgeContainer);
        }

        ConnectConstellations(allMainSectors.SelectMany(i => i), allSubsectors.SelectMany(i => i));
    }

    public IReadOnlyDictionary<GalaxyMapVertex, List<GalaxyMapVertex>> GetEdges()
    {
        return edges;
    }

    private void SetupSubsectors(IEnumerable<GalaxyMapSector> mainSectors, GameObject overallContainer)
    {
        foreach (GalaxyMapSector sector in mainSectors)
        {
            GalaxyMapVertexObjInfo sectorInfo = new GalaxyMapVertexObjInfo(sector, false);
            foreach (GalaxyMapSubsector subsector in sector.GetComponentsInChildren<GalaxyMapSubsector>())
            {
                AddDoubleConnections(sectorInfo, new GalaxyMapVertexObjInfo(subsector, true), overallContainer);
            }
        }
    }

    private void SetupConstellationEdges(GalaxyMapSector[] vertices, IEnumerable<StringPair> edges, string name, GameObject overallContainer)
    {
        GameObject container = AddContainer(name, overallContainer.transform);
        foreach (StringPair edge in edges)
        {
            int firstPos = Int32.Parse(edge.start) - 1;
            int secondPos = Int32.Parse(edge.end) - 1;

            GalaxyMapVertexObjInfo first = new GalaxyMapVertexObjInfo(vertices[firstPos], false);
            GalaxyMapVertexObjInfo second = new GalaxyMapVertexObjInfo(vertices[secondPos], false);
            AddDoubleConnections(first, second, container);
        }
        SetupSubsectors(vertices, container);
    }

    private void ConnectConstellations(IEnumerable<GalaxyMapSector> sectors, IEnumerable<GalaxyMapSubsector> subsectors)
    {
        GameObject interConstellationContainer = AddContainer("Inter-constellation edges", transform);
        GameObject subsectorContainer = AddContainer("Subsector edges", transform);
        Dictionary<string, GalaxyMapVertexObjInfo> nameDict = new Dictionary<string, GalaxyMapVertexObjInfo>();

        foreach (GalaxyMapVertex vertex in sectors)
        {
            nameDict.Add(vertex.GetSectorID(), new GalaxyMapVertexObjInfo(vertex, false));
        }

        foreach (GalaxyMapVertex vertex in subsectors)
        {
            nameDict.Add(vertex.GetSectorID(), new GalaxyMapVertexObjInfo(vertex, true));
        }

        foreach (StringPair pair in Reader.ReadPairedFile(constellationConnectText, "\t"))
        {
            try
            {
                AddDoubleConnections(nameDict[pair.start], nameDict[pair.end], interConstellationContainer);
            }
            catch (KeyNotFoundException e)
            {
                Debug.LogError(string.Format("Couldn't find sector {0} or {1} for constellation connection", pair.start, pair.end));
            }
        }

        foreach (TextAsset text in subsectorConnectTexts)
        {
            foreach (StringPair pair in Reader.ReadPairedFile(text, "\t"))
            {
                try
                {
                    AddDoubleConnections(nameDict[pair.start], nameDict[pair.end], subsectorContainer);
                }
                catch (KeyNotFoundException e)
                {
                    Debug.LogError(string.Format("Couldn't find sector {0} or {1} for subsector edges", pair.start, pair.end));
                }
            }
        }
    }

    public class GalaxyMapVertexObjInfo
    {
        public readonly GalaxyMapVertex vertex;
        public readonly bool isSubsector;

        public GalaxyMapVertexObjInfo(GalaxyMapVertex vertex, bool isSubsector)
        {
            this.vertex = vertex;
            this.isSubsector = isSubsector;
        }
    }

    private void AddDoubleConnections(GalaxyMapVertexObjInfo first, GalaxyMapVertexObjInfo second, GameObject container)
    {
        AddConnection(first.vertex, second.vertex);
        AddConnection(second.vertex, first.vertex);
        AddEdgeObj(first, second, container);
    }

    private GameObject AddContainer(string name, Transform parent)
    {
        GameObject container = new GameObject(name);
        container.transform.SetParent(parent);
        container.transform.localScale = Vector3.one;
        container.transform.localPosition = Vector3.zero;
        return container;
    }

    private void AddEdgeObj(GalaxyMapVertexObjInfo first, GalaxyMapVertexObjInfo second, GameObject container)
    {
        GalaxyMapEdge edge = Instantiate(edgePrefab);
        edge.transform.SetParent(container.transform);
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
}
