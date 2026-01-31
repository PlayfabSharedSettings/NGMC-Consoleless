using System;
using System.Linq;
using System.Reflection;
using NMGC.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NMGC.Core;

// pronounced /ˈɡuː.i/
public class GUIController : MonoBehaviour
{
    public static Transform MainPanel;

    private void Start()
    {
        MainPanel = transform.GetChild(0);

        Type[] nmgcViews = Assembly.GetExecutingAssembly().GetTypes()
                                   .Where(t => t.IsClass                                 && !t.IsAbstract &&
                                               typeof(MonoBehaviour).IsAssignableFrom(t) &&
                                               t.GetCustomAttribute<NMGCViewAttribute>() != null).ToArray();

        foreach (Type nmgcView in nmgcViews)
        {
            string viewName = nmgcView.GetCustomAttribute<NMGCViewAttribute>().ViewName;
            MainPanel.GetChild(1).GetChild(0).GetChild(0).GetChild(0).Find(viewName).GetComponent<Button>().onClick
                     .AddListener(() => ChangeCurrentView(viewName));

            MainPanel.GetChild(2).Find(viewName + "View").gameObject.AddComponent(nmgcView);
        }

        ChangeCurrentView("CustomProperties");

        MainPanel.GetChild(2).gameObject.SetActive(true);
        MainPanel.GetChild(3).gameObject.SetActive(false);

        MainPanel.gameObject.SetActive(false);
    }

    private void ChangeCurrentView(string newViewName)
    {
        foreach (Transform view in MainPanel.GetChild(2))
            view.gameObject.SetActive(view.name == newViewName + "View");
    }
}
