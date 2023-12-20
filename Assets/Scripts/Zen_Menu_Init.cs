using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class Zen_Menu_Init : MonoInstaller
{
    [SerializeField] private Canvas_Menu canvas_manager;

    public override void InstallBindings()
    {
        Container.Bind<Canvas_Menu>().FromInstance(canvas_manager);
    }
}