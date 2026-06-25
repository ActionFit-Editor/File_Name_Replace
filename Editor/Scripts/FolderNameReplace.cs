using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class FileNameReplaceTool : EditorWindow
{
    private DefaultAsset targetFolder;
    private string stringBefore;
    private string stringAfter;
    private string deleteKeyword;

    private enum OperationMode
    {
        Replace,
        Prefix,
        Suffix,
        Delete
    }

    [MenuItem("Tools/ActionFit/File Name Replace", false, 20)]
    public static void Open()
    {
        GetWindow<FileNameReplaceTool>("File Name Replace");
    }

    private void OnGUI()
    {
        GUILayout.Label("폴더 내 파일 이름 일괄 처리", EditorStyles.boldLabel);

        targetFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Target Folder",
            targetFolder,
            typeof(DefaultAsset),
            false
        );

        GUILayout.Space(8);

        stringBefore = EditorGUILayout.TextField("String Before (Replace)", stringBefore);
        stringAfter  = EditorGUILayout.TextField("String After", stringAfter);
        deleteKeyword = EditorGUILayout.TextField("Delete Keyword", deleteKeyword);

        GUILayout.Space(20);

        GUI.enabled = targetFolder != null;

        if (GUILayout.Button("Replace (Before → After)", GUILayout.Height(30)))
            PreviewAndExecute(OperationMode.Replace);

        GUILayout.Space(10);

        if (GUILayout.Button("Add Prefix (맨 앞에 추가)", GUILayout.Height(30)))
            PreviewAndExecute(OperationMode.Prefix);

        GUILayout.Space(10);

        if (GUILayout.Button("Add Suffix (맨 뒤에 추가)", GUILayout.Height(30)))
            PreviewAndExecute(OperationMode.Suffix);

        GUILayout.Space(20);

        if (GUILayout.Button("Delete Files (키워드 포함)", GUILayout.Height(35)))
            PreviewAndExecute(OperationMode.Delete);

        GUI.enabled = true;
    }

    private void PreviewAndExecute(OperationMode mode)
    {
        string folderPath = AssetDatabase.GetAssetPath(targetFolder);
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("유효하지 않은 폴더입니다.");
            return;
        }

        var targets = CollectTargets(folderPath, mode);
        if (targets.Count == 0)
        {
            EditorUtility.DisplayDialog("결과 없음", "대상 파일이 없습니다.", "확인");
            return;
        }

        string preview = string.Join("\n", targets.Take(10));
        if (targets.Count > 10)
            preview += $"\n... 외 {targets.Count - 10}개";

        bool confirm = EditorUtility.DisplayDialog(
            "작업 확인",
            $"작업 종류: {mode}\n대상 파일 수: {targets.Count}\n\n{preview}\n\n정말 적용하시겠습니까?",
            "적용",
            "취소"
        );

        if (!confirm) return;

        ExecuteOperation(targets, mode);
    }

    private List<string> CollectTargets(string folderPath, OperationMode mode)
    {
        var result = new List<string>();
        var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if (file.EndsWith(".meta")) continue;

            string path = file.Replace("\\", "/");
            string name = Path.GetFileNameWithoutExtension(path);

            switch (mode)
            {
                case OperationMode.Replace:
                    if (!string.IsNullOrEmpty(stringBefore) && name.Contains(stringBefore))
                        result.Add(path);
                    break;

                case OperationMode.Prefix:
                    if (!string.IsNullOrEmpty(stringAfter) && !name.StartsWith(stringAfter))
                        result.Add(path);
                    break;

                case OperationMode.Suffix:
                    if (!string.IsNullOrEmpty(stringAfter) && !name.EndsWith(stringAfter))
                        result.Add(path);
                    break;

                case OperationMode.Delete:
                    if (!string.IsNullOrEmpty(deleteKeyword) && name.Contains(deleteKeyword))
                        result.Add(path);
                    break;
            }
        }

        return result;
    }

    private void ExecuteOperation(List<string> targets, OperationMode mode)
    {
        int count = 0;

        foreach (var path in targets)
        {
            string name = Path.GetFileNameWithoutExtension(path);

            switch (mode)
            {
                case OperationMode.Replace:
                    AssetDatabase.RenameAsset(path, name.Replace(stringBefore, stringAfter));
                    break;

                case OperationMode.Prefix:
                    AssetDatabase.RenameAsset(path, stringAfter + name);
                    break;

                case OperationMode.Suffix:
                    AssetDatabase.RenameAsset(path, name + stringAfter);
                    break;

                case OperationMode.Delete:
                    AssetDatabase.DeleteAsset(path);
                    break;
            }

            count++;
        }

        AssetDatabase.Refresh();
        Debug.Log($"작업 완료 ({mode}) : 처리된 파일 수 = {count}");
    }
}
