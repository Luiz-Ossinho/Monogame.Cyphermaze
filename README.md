# Cyphermaze

Cyphermaze is a Monogame project wich aims to make a 3d rendering engine with text-based graphics.
This is a for-fun and for-study project, feel free to copy,add or discuss anything.

## Installation

For developing and building Monogame applications, you will need the following workloads installed in your Visual Studio
- .NET Core cross-platform development - For Desktop OpenGL and DirectX platforms
- Mobile Development with .NET - For Android and iOS platforms (optional, only needed for android development)
- Universal Windows Platform development - For Windows 10 and Xbox UWP platforms
- .Net Desktop Development - For Desktop OpenGL and DirectX platforms to target normal .NET Framework (requires a graphics card that suports opengl 3.3)

For more information regarding Monogame installation, follow this link -> https://docs.monogame.net/articles/getting_started/1_setting_up_your_development_environment_windows.html

The mcgb editor is also required for editing the content pipeline, more info in the link above

MGCB Editor is a tool for editing .mgcb files, which are used for building content.
To register the MGCB Editor tool with Windows and Visual Studio 2019, run the following from the Command Prompt.

```dotnet tool install --global dotnet-mgcb-editor```
```mgcb-editor --register```

## Usage

You can run this application on Debug / x64 -> Local Machine

Plataform-specific code should be added to their own plataform-specific projects,
Abstraction and universal components can be added to the .Core project

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## Technologies

This uses the Monogame framework for creating game-related code.
The rendering uses hlsl for shaders and effects (knowledge of both DirectX and OpenGL are usefull here)
This project is written in C# 8

## License
[MIT](https://choosealicense.com/licenses/mit/)