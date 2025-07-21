using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
// ReSharper disable HeapView.ObjectAllocation
#pragma warning disable

// https://bitbucket.org/snippets/Bjarkeck/keRbr4
namespace CodeBase.Editor
{
  public class ScriptableObjectCreator : OdinMenuEditorWindow
  {
    static readonly HashSet<Type> ScriptableObjectTypes = AssemblyUtilities.GetTypes(AssemblyTypeFlags.CustomTypes)
      .Where(t =>
        t.IsClass &&
        typeof(ScriptableObject).IsAssignableFrom(t) &&
        !typeof(EditorWindow).IsAssignableFrom(t) &&
        !typeof(UnityEditor.Editor).IsAssignableFrom(t))
      .ToHashSet();

    [MenuItem("Assets/Create Scriptable Object", priority = -1000)]
    private static void ShowDialog()
    {
      var path = "Assets";
      var obj = Selection.activeObject;
      if (obj && AssetDatabase.Contains(obj))
      {
        path = AssetDatabase.GetAssetPath(obj);
        if (!Directory.Exists(path))
        {
          path = Path.GetDirectoryName(path);
        }
      }

      var window = CreateInstance<ScriptableObjectCreator>();
      window.ShowUtility();
      window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
      window.titleContent = new GUIContent(path);
      window._targetFolder = path.Trim('/');
    }

    private ScriptableObject _previewObject;
    private string _targetFolder;
    private Vector2 _scroll;

    private Type SelectedType
    {
      get
      {
        var m = this.MenuTree.Selection.LastOrDefault();
        return m == null ? null : m.Value as Type;
      }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
      this.MenuWidth = 270;
      this.WindowPadding = Vector4.zero;

      OdinMenuTree tree = new OdinMenuTree(false);
      tree.Config.DrawSearchToolbar = true;
      tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
      tree.AddRange(ScriptableObjectTypes.Where(x => !x.IsAbstract), GetMenuPathForType).AddThumbnailIcons();
      tree.SortMenuItemsByName();
      tree.Selection.SelectionConfirmed += x => this.CreateAsset();
      tree.Selection.SelectionChanged += e =>
      {
        if (this._previewObject && !AssetDatabase.Contains(this._previewObject))
        {
          DestroyImmediate(this._previewObject);
        }

        if (e != SelectionChangedType.ItemAdded)
        {
          return;
        }

        var t = this.SelectedType;
        if (t != null && !t.IsAbstract)
        {
          this._previewObject = CreateInstance(t) as ScriptableObject;
        }
      };

      return tree;
    }

    private string GetMenuPathForType(Type t)
    {
      if (t != null && ScriptableObjectTypes.Contains(t))
      {
        var name = t.Name.Split('`').First().SplitPascalCase();
        return GetMenuPathForType(t.BaseType) + "/" + name;
      }

      return "";
    }

    protected override IEnumerable<object> GetTargets()
    {
      yield return this._previewObject;
    }

    protected override void DrawEditor(int index)
    {
      this._scroll = GUILayout.BeginScrollView(this._scroll);
      {
        base.DrawEditor(index);
      }
      GUILayout.EndScrollView();

      if (this._previewObject)
      {
        GUILayout.FlexibleSpace();
        SirenixEditorGUI.HorizontalLineSeparator(1);
        if (GUILayout.Button("Create Asset", GUILayoutOptions.Height(30)))
        {
          this.CreateAsset();
        }
      }
    }

    private void CreateAsset()
    {
      if (this._previewObject)
      {
        var dest = this._targetFolder + "/" + this.MenuTree.Selection.First().Name.Replace(" ", "") + ".asset";
        dest = AssetDatabase.GenerateUniqueAssetPath(dest);
        AssetDatabase.CreateAsset(this._previewObject, dest);
        AssetDatabase.Refresh();
        Selection.activeObject = this._previewObject;
        EditorApplication.delayCall += this.Close;
      }
    }
  }
}