# GUPU
Git Unity Package Updater: an editor tool for updating git UPM packages.
###### Installation
For Unity 2019.4+, You can install the package through the Unity Package Manager (UPM):

1. `Window -> Package Manager`
2. `+ (Add package button) -> Add Package from git URL`
3. Use the URL `https://github.com/rellfy/GUPU.git`

Please note that GUPU requires [Newtonsoft.Json](https://www.newtonsoft.com/json) to be available within the assembly.  
You must also have [git](https://git-scm.com/) installed and available from your PATH environment variable.
###### Usage
Open the control panel from `Window -> Git Package Updater`
###### How it works
The package object in `Packages/packages-lock.json` is updated to reflect the HEAD commit hash as obtained from `git ls-remote`. After the updated lockfile is written to the file system, Unity itself handles the updating of the package via UPM.