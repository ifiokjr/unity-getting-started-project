# Making A Unity Project Ready For Git

These instructions are only applicable for the Mac system.

Firstly install git-extras

```bash
brew install git-extras
```

If you don’t use iterm (which is a great terminal program) install it as follow
brew cask install iterm

Now navigate to the project of the unity folder. A simple way is to drag the folder from finder into your terminal of choice to get the directory.

```
git init # initialize as a git project
git ignore-io Unity JetBrains macOS > .gitignore # Add standard files to the gitignore folder
```

### Git LFS

For git lfs support which is a good thing I suppose

```
brew install git-lfs
git lfs install # In your repo - please note that this conflicts with any git hooks already added
```

You can follow instructions outlined here for handling empty folders https://github.com/strich/git-dir-cleaner-for-unity3d

Alternatively copy the following

```bash
#!/bin/sh

# Command line options:
isSquashMerge="$1"
dirsRemovedCounter=0
echo "Removing empty directories..."

# Grab a list of deleted files:
changedFiles="$(git diff-tree -r --name-only --diff-filter=D --no-commit-id ORIG_HEAD HEAD)"

# Early exit if there are no removed files at all:
if [ -z "$changedFiles" ]; then
exit 0
fi

# Get the list of dir paths and then sort and remove dupes:
dirsToCheck="$(echo "$changedFiles" | xargs -d '\n' dirname | sort -u)"
# For each dir check if its empty and if so, remove it:
for dir in $dirsToCheck; do
if [ ! -d "$dir" ]; then
continue
fi
((dirsRemovedCounter++))
find "$dir" -type d -empty -delete
done
echo "Removed" $dirsRemovedCounter "directories."

## Git LFS
command -v git-lfs >/dev/null 2>&1 || { echo >&2 "\nThis repository is configured for Git LFS but 'git-lfs' was not found on your path. If you no longer wish to use Git LFS, remove this hook by deleting .git/hooks/pre-push.\n"; exit 2; }
git lfs pre-push "$@"
```

And then run this command from the directory.

`pbpaste > .git/hooks/post-merge`

### Project Git Ready

1.  Open the Edit menu and pick Project Settings → Editor:
1.  Switch Version Control Mode to Visible Meta Files.
1.  Switch Asset Serialization Mode to Force Text.

If the game is a mobile game adding firebase support is as simple as following these instructions https://firebase.google.com/docs/unity/setup.

Create a .gitconfig file and add the following for automated merge support

```yaml
[merge]
tool = unityyamlmerge

[mergetool "unityyamlmerge"]
trustExitCode = false
cmd = '/Applications/Unity/Unity.app/Contents/Tools/UnityYAMLMerge' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
```

Create a .gitattributes file which accounts for all the file types that unity currently support to add git-lfs support.

```text
# 3D models
*.3dm filter=lfs diff=lfs merge=lfs -text
*.3ds filter=lfs diff=lfs merge=lfs -text
*.blend filter=lfs diff=lfs merge=lfs -text
*.c4d filter=lfs diff=lfs merge=lfs -text
*.collada filter=lfs diff=lfs merge=lfs -text
*.dae filter=lfs diff=lfs merge=lfs -text
*.dxf filter=lfs diff=lfs merge=lfs -text
*.fbx filter=lfs diff=lfs merge=lfs -text
*.jas filter=lfs diff=lfs merge=lfs -text
*.lws filter=lfs diff=lfs merge=lfs -text
*.lxo filter=lfs diff=lfs merge=lfs -text
*.ma filter=lfs diff=lfs merge=lfs -text
*.max filter=lfs diff=lfs merge=lfs -text
*.mb filter=lfs diff=lfs merge=lfs -text
*.obj filter=lfs diff=lfs merge=lfs -text
*.ply filter=lfs diff=lfs merge=lfs -text
*.skp filter=lfs diff=lfs merge=lfs -text
*.stl filter=lfs diff=lfs merge=lfs -text
*.ztl filter=lfs diff=lfs merge=lfs -text
# Audio
*.aif filter=lfs diff=lfs merge=lfs -text
*.aiff filter=lfs diff=lfs merge=lfs -text
*.it filter=lfs diff=lfs merge=lfs -text
*.mod filter=lfs diff=lfs merge=lfs -text
*.mp3 filter=lfs diff=lfs merge=lfs -text
*.ogg filter=lfs diff=lfs merge=lfs -text
*.s3m filter=lfs diff=lfs merge=lfs -text
*.wav filter=lfs diff=lfs merge=lfs -text
*.xm filter=lfs diff=lfs merge=lfs -text
# Fonts
*.otf filter=lfs diff=lfs merge=lfs -text
*.ttf filter=lfs diff=lfs merge=lfs -text
# Images
*.bmp filter=lfs diff=lfs merge=lfs -text
*.exr filter=lfs diff=lfs merge=lfs -text
*.gif filter=lfs diff=lfs merge=lfs -text
*.hdr filter=lfs diff=lfs merge=lfs -text
*.iff filter=lfs diff=lfs merge=lfs -text
*.jpeg filter=lfs diff=lfs merge=lfs -text
*.jpg filter=lfs diff=lfs merge=lfs -text
*.pict filter=lfs diff=lfs merge=lfs -text
*.png filter=lfs diff=lfs merge=lfs -text
*.psd filter=lfs diff=lfs merge=lfs -text
*.tga filter=lfs diff=lfs merge=lfs -text
*.tif filter=lfs diff=lfs merge=lfs -text
*.tiff filter=lfs diff=lfs merge=lfs -text
```

And for better git diff support as explained here append the following to you .gitattributes file as well

```text
# Collapse Unity-generated files on GitHub

_.asset linguist-generated
_.mat linguist-generated
_.meta linguist-generated
_.prefab linguist-generated
\*.unity linguist-generated
```
