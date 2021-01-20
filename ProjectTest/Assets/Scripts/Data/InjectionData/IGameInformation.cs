using UnityEngine;

namespace Assets.Scripts.Data.InjectionData
{
    public interface IGameInformation
    {
        GameObject Player { get; set; }

        Camera Camera { get; set; }
    }
}
