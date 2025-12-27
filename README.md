# WebXR AR Greeting Card

This project showcases an augmented reality (AR) greeting card experience using Unity and WebXR. Accessible through web browsers, the AR card is designed for compatible Android and XR devices (e.g., Quest headsets), offering unique, interactive cards for various occasions like birthdays, Halloween, and Christmas.

## Project Overview

The AR Greeting Card enables users to view and interact with a 3D card in their real-world environment using their mobile device's camera. With no app installation required, this project is easily accessible and shareable, making it a convenient way to send themed greetings.

## Demo

Explore the AR Greeting Card in action: [AR Greeting Card Demo](https://97saundersj.github.io/cARds). The demo allows users to choose from customizable cards for birthdays, Halloween, and Christmas.

## Sharing Your AR Greeting Card

After customizing your AR greeting card, you can generate a shareable link. Click the "Save & Generate Link" button, and the link will be copied to your clipboard. You can then share this link with others to allow them to view your customized card.

## Technical Stack

- Unity
- WebXR
- WebGL
- HTML/CSS/JavaScript

## Development

### Running the Unity WebGL Build Locally

To serve the Unity WebGL build locally without CORS issues:

1. Navigate to your Unity build location:

```powershell
cd <your-unity-build-folder>
```

2. Run the http-server with CORS enabled:

```powershell
npx http-server -p 8000 --cors
```

3. Access at: `http://localhost:8000`

**Note:** The `--cors` flag ensures proper CORS headers are sent, which is essential for Unity WebGL builds to work correctly when loaded from different origins.

## Features

- Customizable landing page and greeting card themes
- Interactive AR experience in a real-world setting
- No app installation required
- Compatible with modern web browsers on Android and XR devices

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue with suggestions for improvements or new features.
