using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class FileList : Control
{
    [Export] PackedScene folderSection;
    VBoxContainer vbFileLists;
    FolderSection looseFilesList;
    public List<string> AllFiles {get; private set;}

    public Dictionary<string, List<string>> fileStack = new Dictionary<string, List<string>>() {
        {"Loose Files", new List<string>()}
    };

    List<string> GetAllFiles() {
        List<string> l = new List<string>();
        foreach (KeyValuePair<string, List<string>> fileLst in fileStack) {
            foreach (string file in fileLst.Value) {
                l.Add($"{fileLst.Key}>{file}"); // string before > is the rootpath, after it the filepath"
            }
        }
        return l;
    }

    
    public void OnBeforeEvaluation() {

        if (GetParent<TabContainer>().CurrentTab == GetIndex()) {
            AllFiles = GetAllFiles();
            Project.MaxNumFiles = AllFiles.Count;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        vbFileLists = GetNode<VBoxContainer>("Scroll/VBFileList");
        looseFilesList = GetNode<FolderSection>("Scroll/VBFileList/FSLooseFilesList");
        AddToGroup(FNode.RunBeforeEvaluationGroup);
    }

    public void ShowFolderDropdown(bool show) {
        //GetNode<ItemList>("List").Visible = show;
    }

    public void RemoveSelectedItems() {
        foreach (FolderSection fs in vbFileLists.GetChildren()) {
            int[] selItems = fs.FileList.GetSelectedItems();
            int i=0;
            foreach (int item in selItems) {
                fs.FileList.RemoveItem(item-i);
                try {
                    fileStack[fs.ConnecedFolder].RemoveAt(item-i);
                } catch (System.Exception e) {
                    Errorlog.Log(e);
                }
                i++;
            }
        }

        //UpdateUIList();
    }
    

    public void UpdateUIListAtKey(string key) {
        foreach (FolderSection fs in vbFileLists.GetChildren()) {
            if (fs.ConnecedFolder == key) {
                fs.FileList.Clear();
                fs.AddFiles(fileStack[key]);
                }
        }
    }

    public void UpdateUIList() {
        List<int> collapsed = new List<int>{};
        List<int> nonRecursive = new List<int>{};
        foreach (FolderSection fs in vbFileLists.GetChildren()) {
            if (!fs.Cb.IsPressed()) {
                collapsed.Add(fs.GetIndex());
            }
            if (!fs.CbRecursive.IsPressed()) {
                nonRecursive.Add(fs.GetIndex());
            }
            fs.QueueFree();
        }

        int i = 0;
        foreach (var dictItem in fileStack) {
            string dirPath = dictItem.Key;
            FolderSection fs = folderSection.Instantiate<FolderSection>();
            fs.ConnecedFolder = dirPath;
            vbFileLists.AddChild(fs);
            fs.Cb.Text = UIUtil.GetOverflowDots(dirPath.GetFile(), fs.Cb.GetThemeFont("normal"), 185);
            fs.Cb.TooltipText = dirPath;
            fs.AddFiles(dictItem.Value);

            if (collapsed.Contains(i)) {
                fs.Cb.SetPressed(false);
                fs.FileList.Visible = false;
            }

            //Hide Recusrive Button from Loose Files Section
            bool notLoosFiles = i != 0;
            fs.CbRecursive.Visible = notLoosFiles;
            fs.BtnReload.Visible = notLoosFiles;

            if (notLoosFiles) {
                fs.CbRecursive.SetPressed(!nonRecursive.Contains(i));
            }
            i++;
        }
    }

    public void AddFiles(string [] paths, bool recursive = true) {

        if (Name=="0") {
            GetNode<Label>("LblDragnDrop").Visible = false;
        }
        Main.preventRun = true;

        int i=0;
        foreach (string path in paths) {
            
            string baseDirAdded = "";
            bool alreadyAdded = false;
            bool isDir = System.IO.Directory.Exists(path);

            string baseDirFile = isDir ? path : path.GetBaseDir();

            //Check if folder to add is a sub- or parentfolder of an already existed folder. If yes, don't allow adding it.
            foreach (var folder in fileStack) {
                if (baseDirFile.Contains(folder.Key) || folder.Key.Contains(path)) { //Might be unsave (using / or \ etc..)
                    baseDirAdded = folder.Key;
                    InfoLine.Show($"Cannot add directory {path} as it (or a parent / subdirectory) is already added to the Stack");
                    alreadyAdded = true;
                    break;
                }
            }

            if (alreadyAdded) {
                    continue;
            }

            if (isDir) {
                //Check if folder to add is a sub- or parentfolder of an already existed folder. If yes, don't allow adding it.

                string[] dirFiles;
                if (recursive) {
                    dirFiles = System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories);
                } else {
                    dirFiles = System.IO.Directory.GetFiles(path);
                }
                fileStack.Add(path, dirFiles.ToList<string>());
            }
            
            else {

                if (baseDirAdded != "") { //TODO currently doesn't account for recursively loaded files...
                    if (fileStack[baseDirAdded].Contains(path)) {
                        continue;
                    }
                    fileStack[baseDirAdded].Add(path);
                    i++;
                    InfoLine.Show($"Added {i} files");
                    //Task.Delay(1).Wait();
                } 
                else {
                    if (fileStack["Loose Files"].Contains(path)) {
                        continue;
                    }
                    fileStack["Loose Files"].Add(path);
                    i++;
                    InfoLine.Show($"Added {i} files");
                    Task.Delay(1).Wait();
                }
            }
        }
        UpdateUIList();
        Main.preventRun = false;
    
    }
}
