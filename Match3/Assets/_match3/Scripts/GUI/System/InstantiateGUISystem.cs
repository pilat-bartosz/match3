using Unity.Entities;
using UnityEngine;

namespace _match3.GUI
{
    [UpdateInGroup(typeof(GUISystemGroup))]
    public partial class InstantiateGUISystem : SystemBase
    {
        private EntityQuery _instantiateQuery;

        protected override void OnCreate()
        {
            _instantiateQuery = SystemAPI.QueryBuilder()
                .WithAll<GUIPrefab>()
                .WithNone<GUIManager, GUIReference>()
                .Build();

            RequireForUpdate(_instantiateQuery);
        }

        protected override void OnUpdate()
        {
            var guiPrefabEntity = _instantiateQuery.GetSingletonEntity();
            var guiPrefab = _instantiateQuery.GetSingleton<GUIPrefab>();

            var guiObject = Object.Instantiate(guiPrefab.guiPrefab);
            EntityManager.AddComponentObject(guiPrefabEntity, guiObject);
            EntityManager.AddComponentObject(guiPrefabEntity, new GUIReference
            {
                guiReference = guiObject
            });
        }
    }
}