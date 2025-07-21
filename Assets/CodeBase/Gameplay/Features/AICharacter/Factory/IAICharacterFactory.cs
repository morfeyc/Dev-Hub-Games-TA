using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Gameplay.Features.AICharacter.Factory
{
  public interface IAICharacterFactory
  {
    UniTask InitializeAsync();
    AICharacter CreateAICharacter(Vector3 spawnPosition);
    void DespawnAICharacter(AICharacter aiCharacter);
    void Cleanup();
  }
}