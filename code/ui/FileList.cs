using Godot;
using System.Collections.Generic;
using System.Linq;

public class FileList : Control
{
    [Export] PackedScene folderSection;
    VBoxContainer vbFileLists;
    FolderSection looseFilesList;

    public Dictionary<string, List<string>> fileStack = new Dictionary<string, List<string>>() {
        {"Loose Files", new List<string>()}
    };

    public List<string> GetAllFiles() {
        List<string> l = new List<string>();
        foreach (KeyValuePair<string, List<string>> fileLst in fileStack) {
            l.AddRange(fileLst.Value);
        }
        return l;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        vbFileLists = GetNode<VBoxContainer>("Scroll/VBFileList");
        looseFilesList = GetNode<FolderSection>("Scroll/VBFileList/FSLooseFilesList");
    }

    public void ShowFolderDropdown(bool show) {
        //GetNode<ItemList>("List").Visible = show;
    }

    public void RemoveSelectedItems() {
        foreach (FolderSection fs in vbFileLists.GetChildren()) {
            int[] selItems = fs.list.GetSelectedItems();
            int i=0;
            foreach (int item in selItems) {
                fs.list.RemoveItem(item-i);
                try {
                    fileStack[fs.connecedFolder].RemoveAt(item-i);
                } catch (System.Exception e) {
                    Errorlog.Log(e);
                }
                i++;
            }
        }

        //UpdateUIList();
    }

    public void UpdateUIList() {
        List<int> collapsed = new List<int>{};
        foreach (FolderSection fs in vbFileLists.GetChildren()) {
            if (!fs.cb.Pressed) {
                collapsed.Add(fs.GetIndex());
            }
            fs.QueueFree();
        }

        int i = 0;
        foreach (var dictItem in fileStack) {
            string dirPath = dictItem.Key;
            FolderSection fs = folderSection.Instance<FolderSection>();
            fs.connecedFolder = dirPath;
            vbFileLists.AddChild(fs);
            fs.cb.Text = dirPath.GetFile();
            fs.AddFiles(dictItem.Value);

            if (collapsed.Contains(i)) {
                fs.cb.Pressed = false;
                fs.list.Visible = false;
            }
            i++;
        }
    }

    public void AddFiles(string [] paths) {
        foreach (string path in paths) {
            bool isDir = System.IO.Directory.Exists(path);

            if (isDir && !fileStack.ContainsKey(path)) {
                

                string[] dirFiles = System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories);
                fileStack.Add(path, dirFiles.ToList<string>());
            } 
            
            else if (!isDir) {
                string baseDir = path.GetBaseDir(); 

                string baseDirAdded = "";
                foreach (var folder in fileStack) {
                    if (baseDir.Contains(folder.Key)) { //Might be unsave (using / or \ etc..)
                        baseDirAdded = folder.Key;
                        break;
                    }
                }

                if (baseDirAdded != "") { //TODO currently doesn't account for recursively loaded files...
                    if (fileStack[baseDirAdded].Contains(path)) {
                        continue;
                    }
                    fileStack[baseDirAdded].Add(path);
                } 
                else {
                    if (fileStack["Loose Files"].Contains(path)) {
                        continue;
                    }
                    fileStack["Loose Files"].Add(path);
                }
            }
        }
        UpdateUIList();
    }
}
    