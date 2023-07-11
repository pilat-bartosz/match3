using Unity.Entities;
using UnityEngine;

namespace _match3.GUI
{
    public class GUIReference : ICleanupComponentData
    {
        public GUIManager guiReference;
    }
}