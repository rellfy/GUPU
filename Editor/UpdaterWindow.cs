using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUPU {
    public class UpdaterWindow : EditorWindow {

        private static Dictionary<string, PackageLock.Dependency> LockFile;
        
        [MenuItem("Window/Git Package Updater")]
        public static void ShowWindow() {
            EditorWindow window = GetWindow<UpdaterWindow>();
            window.titleContent = new GUIContent("GUPU");
            window.minSize = new Vector2(400, 350);
        }

        private void UpdateLockfile() {
            LockFile = PackageLock.File;
        }

        private void OnEnable() {
            Render();
        }

        private void Render() {
            rootVisualElement.Clear();
            UpdateLockfile();
            DisplayPackages();
        }

        private void DisplayPackages() {
            int count = 0;
            
            ScrollView scrollView = new ScrollView();
            scrollView.style.width = new StyleLength(Length.Percent(100));
            scrollView.style.height = new StyleLength(Length.Percent(100));
            
            foreach (KeyValuePair<string, PackageLock.Dependency> entry in LockFile) {
                if (entry.Value.source != "git")
                    continue;
                VisualElement package = RenderPackage(entry.Key, entry.Value);
                count += 1;
                scrollView.Add(package);
            }
            
            if (count == 0) {
                TextElement message = new TextElement();
                message.text = "no git packages found";
                scrollView.Add(message);
            }
            
            rootVisualElement.Add(scrollView);
        }

        private VisualElement RenderPackage(string name, PackageLock.Dependency package) {
            Box row = new Box();
            row.style.borderTopWidth = new StyleFloat(0f);
            row.style.borderRightWidth = new StyleFloat(0f);
            row.style.borderBottomWidth = new StyleFloat(0f);
            row.style.borderLeftWidth = new StyleFloat(0f);
            row.style.flexDirection = FlexDirection.Row;
            row.style.minHeight = new StyleLength(25f);
            TextElement packageName = new TextElement();
            packageName.text = name;
            packageName.style.width = new StyleLength(new Length(50, LengthUnit.Percent));
            packageName.style.unityTextAlign = new StyleEnum<TextAnchor>(
                TextAnchor.MiddleCenter
            );
            packageName.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            row.Add(packageName);
            VisualElement status;
            if (IsPackageUpdated(package, out string hash)) {
                TextElement updatedText = new TextElement();
                updatedText.text = "already updated";
                updatedText.style.unityTextAlign = new StyleEnum<TextAnchor>(
                    TextAnchor.MiddleCenter
                );
                status = updatedText;
            } else {
                Button update = new Button();
                update.text = "Update";
                update.clickable.clicked += () => UpdatePackage(name, hash);
                status = update;
            }
            status.style.width = new StyleLength(new Length(50, LengthUnit.Percent));
            row.Add(status);
            return row;
        }

        private bool IsPackageUpdated(PackageLock.Dependency package, out string hash) {
            hash = Updater.GetLastCommitHash(package.version);
            return package.hash == hash;
        }

        private void UpdatePackage(string name, string newHash) {
            if (!LockFile.TryGetValue(name, out PackageLock.Dependency package))
                throw new Exception("Package not found in cached lockfile");
            if (LockFile[name].source != "git")
                throw new Exception("Cannot update non-git package");
            package.hash = newHash;
            LockFile[name] = package;
            PackageLock.File = LockFile;
            Render();
        }
    }
}