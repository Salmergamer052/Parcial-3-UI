using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] MenuVisibility[] menus;

    public static MenuManager Instance;


    public void Awake()
    {
        Instance = this;
    }

    public void OpenMenuName(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].nameMenu == menuName)
            {
                menus[i].Visible();
            }
            else if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(MenuVisibility menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Visible();
    }
    public void CloseMenu(MenuVisibility menu)
    {
        menu.NoVisible();
    }


}
