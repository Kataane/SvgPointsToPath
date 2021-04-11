<div align="center">
    <img src="static/800px-SVG_Logo.svg.png" alt="SvgPointsToPath Logo" title="Aimeos" height="150"/>
 <h1>Svg Points to Path</h1>
</div>

Svg Points to Path helps to turn many svg files into one `.cs` type file-storage. 

The file-storage is needed to create icons in Blazor projects. Svg Points to Path can work with all type svg geometry like: **polygon, polyline** and etc into Path.

## 📥 Install
1. Install [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)

2. Clone the repo
```
git clone https://github.com/TwoChisel/SvgPointsToPath.git
cd SvgPointsToPath
```

3. Build the project
```
dotnet build
```

4. Use
```
cd bin\Debug\net5.0
SvgPointsToPath.exe -h
```

## 🔧 Use

Svg Points to Path support next args:
```
-h - show description about main args
'Path' - full path to the folder with svg files
'NameSpace' - NameSpace class where store svg
'Class' - Class name where store svg
'Output' - Path where will be created cs file with svg
```

Next example SvgPointsToPath get test svg files from svg folder in repo. 
Create file-storage with namespace **TwoChisel** and class **Storage** in repo folder.
```
SvgPointsToPath.exe ../../../svg TwoChisel Storage ../../../
```

If everything is ok, you will see the message
```
[31.02.2021 04:20:00]: Well Done! File svg-storage with name Storage created by next path: ../../../Storage.cs
```

## ✈️ Use in Blazor

1.More about [raw HTML](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-3.1#raw-html%5D) in Blazor.
```
<svg>@((MarkupString)Storage.Add)</svg>
```

Where `Storage` class is the generated file from the **previous topic**. And `Add` is icon from the test directory `svg` in the repo.
After generate page `Storage.Add` will be replaced to the path.

## ✅ TODO
 - [ ] Add tests
 - [ ] Rework args
 - [ ] Check invalid characters in the field
 - [ ] Add logs
 
## 📝 License 
[The MIT License (MIT)](https://mit-license.org/)

Made with love by TwoChisel 💜

