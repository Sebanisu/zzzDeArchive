# zzzDeArchive
This tool is used to create/extract/merge zzz archives for FF8 Remastered.</br>
I wrote the c# code and [MakiPL](https://github.com/MaKiPL) did the reversing of the zzz format.

The __Releases__ are for __Windows__ but the source should compile on __mono__ just fine.</br> Uses the `System.ValueTuple` NuGet package.

When you run the __zzzDeArchive.exe__ it'll show a menu. Just follow the prompts and you should have no issue.
```
            --- Welcome to the zzzDeArchive 0.1.7.5 ---
     Code C# written by Sebanisu, Reversing and Python by Maki

1) Extract - Extract zzz file
2) Write - Write folder contents to a zzz file
3) Merge - Write unique data from two or more zzz files into one zzz file.

Escape) Exit

  Select:
```

Now has a gui version.<p align="center">
![GUI version image](https://user-images.githubusercontent.com/1035905/65191880-5a0d5d00-da43-11e9-91c4-df7d9dbe4bfc.png)</p>
The new gui is happy to replace the files in your final fantasy VIII remastered folder. So make sure to back up.

Example of new gui in action:<p align="center">
[![](http://img.youtube.com/vi/bCKF29iVi_c/0.jpg)](http://www.youtube.com/watch?v=bCKF29iVi_c "https://i.ytimg.com/vi/bCKF29iVi_c/hqdefault.jpg?sqp=-oaymwEZCPYBEIoBSFXyq4qpAwsIARUAAIhCGAFwAQ==&rs=AOn4CLAI_1oEDQA1dqv2v4gXsXwW0XUoqA")</p>

There is some command line support as well.

Extract zzz to folder.<br/>
`zzzDeArchive "path to zzz file" "path to folder"`or press __1__ at the main menu.<br/>

Write folder to __out.zzz__.<br/>
`zzzDeArchive "path to folder"`or press __2__ at the main menu.<br/>
Output will be in the __OUT__ folder.

Merge two or more zzz files to __out.zzz__.<br/>
Will attempt to detect __main.zzz__ or __other.zzz__ and change the name accordingly.<br/>
`zzzDeArchive "path to zzz with old data" "path to zzz with new data"` or press __3__ at the main menu.<br/>
Output will be in the __OUT__ folder.

`-skipwarning`
Will not stop and warn you if you are adding files instead of just replacing them.

Folder Merge:<br/>
`zzzDeArchive -foldermerge` or press __4__ at the main menu.<br/>
Optional Arguments:
`zzzDeArchive -foldermerge "path.to\main.zzz" "path.to\other.zzz"`<br/>

- Folders in the `IN\main` and `IN\other` will be treated as unarchived mods. These will be turned into ___folder-name_.zzz__ files. This will overwrite existing zzz files.<br/>
- Place mod zzz files that are to go into __main.zzz__ into `IN\main`<br/>
- Place mod zzz files that are to go into __other.zzz__ into `IN\other`<br/>
- Output will be in the __OUT__ folder.

See similar project: [qt-zzz](https://github.com/myst6re/qt-zzz)

Fourm link: [Qhimm](http://forums.qhimm.com/index.php?topic=19209.msg267708#msg267708)


Building on Linux:
```sh
        wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O /tmp/packages-microsoft-prod.deb
        sudo dpkg -i /tmp/packages-microsoft-prod.deb
        sudo apt update
        sudo apt-get install -y apt-transport-https
        sudo apt-get install -y dotnet-sdk-5.0
        sudo apt install gnupg ca-certificates
        sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
        echo "deb https://download.mono-project.com/repo/ubuntu stable-focal main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
        sudo apt update
        sudo apt install -y mono-devel
        msbuild
```
