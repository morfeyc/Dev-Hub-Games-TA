using System;
using System.Collections.Generic;
using CodeBase.Utils.SmartDebug;

namespace CodeBase.Gameplay.Features.AICharacter.Provider
{
  public class AICharactersProvider : IAICharactersProvider, IDisposable
  {
    public List<AICharacter> ActiveAICharacters { get; private set; } = new();

    public void AddAICharacter(AICharacter ai)
    {
      if (!ActiveAICharacters.Contains(ai))
      {
        ActiveAICharacters.Add(ai);
      }
      else
      {
        DLogger.Message(DSenders.Empty)
          .WithText("Attempted to add an AI character that is already in the provider!")
          .WithFormat(DebugFormat.Warning)
          .Log();
      }
    }

    public void RemoveAICharacter(AICharacter ai)
    {
      ActiveAICharacters.Remove(ai);
    }

    public void ClearAll()
    {
      ActiveAICharacters.Clear();
    }

    public void Dispose()
    {
      ClearAll();
    }
  }
}