using UnityEngine;

namespace RightSoft.Scripts.ScriptableObjects
{
    [CreateAssetMenu]
    public class PatcherSettings : ScriptableObject
    {
        public string RemoteUrl;
        public string LauncherExecutableName;
        public string GameExecutableName;
    }
}