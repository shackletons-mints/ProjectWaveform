using System;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;

public class MetaAgeCategoryHandler : MonoBehaviour
{

    private void Start()
    {
        if (!Oculus.Platform.Core.IsInitialized())
        {
            Oculus.Platform.Core.Initialize();
        }
        UserAgeCategory.Get();
    }
}
