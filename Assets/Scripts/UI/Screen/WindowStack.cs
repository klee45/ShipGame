using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindowStack : Singleton<WindowStack>
{
    [SerializeField]
    private List<IWindow> stack;

    protected override void Awake()
    {
        base.Awake();
        stack = new List<IWindow>();
    }

    public void AddNewWindow<T>(T window) where T : MonoBehaviour, IWindow
    {
        window.transform.SetAsLastSibling();
        window.Show();
        stack.Add(window);
    }

    public void CloseTopWindow()
    {
        stack.Last().Hide();
        stack.RemoveAt(stack.Count - 1);
    }

    public bool CloseWindow(IWindow window)
    {
        bool result = stack.Remove(window);
        if (result)
        {
            window.Hide();
            return true;
        }
        else
        {
            return false;
        }
    }
}
