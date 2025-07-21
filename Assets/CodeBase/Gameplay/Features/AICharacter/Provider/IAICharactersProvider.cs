using System.Collections.Generic;

namespace CodeBase.Gameplay.Features.AICharacter.Provider
{
  public interface IAICharactersProvider
  {
    List<AICharacter> ActiveAICharacters { get; }
    void AddAICharacter(AICharacter ai);
    void RemoveAICharacter(AICharacter ai);
    void ClearAll();
  }
}