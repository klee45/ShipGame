using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class GalaxyMapVertex : MonoBehaviour
{
    [SerializeField]
    private string sectorID;
    [SerializeField]
    protected string sectorName;

    private string sceneName;
    [SerializeField]
    private Vector3 spacePosition;
    private float spaceScale;

    [SerializeField]
    private EmpireStrengths strengths;

    [System.Serializable]
    private struct EmpireStrengths
    {
        public float empire;
        public float federation;
        public float avalon;

        public EmpireStrengths(float galaxyRadius, Vector2 pos, Vector2 direction, float percentInner, string name)
        {
            avalon = 1 - Mathf.Min(1, pos.magnitude / (galaxyRadius * percentInner));
            Debug.Log(name + "\n" + pos + "\n" + Vector2.Dot(pos, direction));
            federation = (1 - avalon) * (1 + (Vector2.Dot(pos, direction) / galaxyRadius)) / 2f;
            empire = 1 - federation - avalon;
        }
    }

    protected virtual void Awake()
    {
        sectorName = SetupName(sectorID);
        this.name = sectorName;
        GetComponentInChildren<Text>().text = sectorID;
        sceneName = SetSceneName();
        spacePosition = SetSpacePosition();
        spaceScale = SetSpaceScale();
    }

    private void Start()
    {
        GalaxyInfo info = GalaxyInfo.instance;
        strengths = new EmpireStrengths(info.GetRadius(), spacePosition, info.GetDirectionVector(), info.GetPercentInner(), name);
    }

    protected abstract string SetupName(string sectorID);
    protected abstract Color GetTextUnhighlighted();
    protected abstract Color GetImageUnhighlighted();
    protected abstract string SetSceneName();
    protected abstract Vector3 SetSpacePosition();
    protected abstract float SetSpaceScale();

    public Vector3 GetSpacePosition()
    {
        return spacePosition;
    }

    public float GetSpaceScale()
    {
        return spaceScale;
    }

    public virtual void SetupMap() { }

    private void OnValidate()
    {
        sectorName = SetupName(sectorID);
        this.name = sectorName;
        GetComponentInChildren<Text>().text = sectorID;
        Unhighlight();
    }

    public void Highlight()
    {
        GetComponentInChildren<Text>().color = Color.white;
        GetComponent<Image>().color = Color.white;
    }

    public void Unhighlight()
    {
        GetComponentInChildren<Text>().color = GetTextUnhighlighted();
        GetComponent<Image>().color = GetImageUnhighlighted();
    }

    public string GetSceneName()
    {
        return sceneName;
    }

    public void SetSectorNameDebugging(string sectorName)
    {
        this.sectorName = sectorName;
    }

    public string GetSectorName()
    {
        return sectorName;
    }

    public string GetSectorID()
    {
        return sectorID;
    }
}
