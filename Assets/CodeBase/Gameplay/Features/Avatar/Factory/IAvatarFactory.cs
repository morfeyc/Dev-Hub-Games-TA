using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.Features.Avatar.Factory
{
  public interface IAvatarFactory
  {
    UniTask<Avatar> CreateAvatar(Vector3 position, Quaternion rotation);
  }
}