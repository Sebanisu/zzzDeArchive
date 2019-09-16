# zzzDeArchive
This tool is used to create/extract/merge zzz archives for FF8 Remastered.</br>
I wrote the c# code and [MakiPL](https://github.com/MaKiPL) did the reversing of the zzz format.

The __Releases__ are for __Windows__ but the source should compile on __mono__ just fine.</br> Uses the `System.ValueTuple` NuGet package.

When you run the __zzzDeArchive.exe__ it'll show a menu. Just follow the prompts and you should have no issue.<p align="center">
![Main menu image](https://raw.githubusercontent.com/Sebanisu/zzzDeArchive/master/img/mainmenu.png)</p>

There is some command line support as well.

Extract zzz to folder.<br/>
`zzzDeArchive "path to zzz file" "path to folder"`

Write folder to __out.zzz__.<br/>
`zzzDeArchive "path to folder"`

Merge two or more zzz files to __out.zzz__.<br/>
`zzzDeArchive "path to zzz with old data" "path to zzz with new data" "path to zzz with new data"...`
If there is a conflict the right most file will override the left files.

See similar project: [qt-zzz](https://github.com/myst6re/qt-zzz)

Fourm link: [Qhimm](http://forums.qhimm.com/index.php?topic=19209.msg267708#msg267708)
