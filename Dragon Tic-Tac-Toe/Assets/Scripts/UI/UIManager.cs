using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public void Initialize()
        {
            Instance ??= this;
        }

        public void ShowResult()
        {

        }
    }
}
