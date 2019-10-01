using UnityEngine;

namespace Core.Runtime.Services
{
    public interface IServiceRoot
    {
        Transform Root { get; }
    }
}