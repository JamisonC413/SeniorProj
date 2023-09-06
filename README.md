# Algorithmic Art Sample Code
This code is an end-to-end sample of the underlying functionality required for this project. Included is a sample web application built with React and Django as well as a sample Unity project that communicates with the frontend of the web application. We recommend building your project off of this sample.

In this README we have provided some installation instructions to get your project off the ground as well as some useful information about the technologies being used.

Contents:
- [Installation](#installation-instructions)
- [Technology Notes](#notes-on-technologies)

---
## Installation Instructions
### Software Requirements
You must have the following installed before running the different parts of this project:

**Web App**
* Docker
* Node.js
* Python3
* MySQL

**Unity**
* Unity Hub
* Unity Editor Version 2022.3.6f1

### Web App Installation
Add an **.env** file at the root of the `web-app` folder, with the following contents, making sure to add values for each
of the empty fields. These values will be loaded into the backend and used to access the database we will be using.

```
MYSQL_HOST=database # Must match database service in docker-compose.yml
MYSQL_PORT=3306
MYSQL_DATABASE=algoarttechstack
MYSQL_USER=
MYSQL_PASSWORD=
MYSQL_ROOT_PASSWORD=
```

Next, install the frontend dependencies:

```
$ cd frontend
$ npm install
```

Next, start up the project Docker containers. This may take a second.

```
$ cd ..
$ docker-compose up --build
```

Once the frontend has finished building, navigate to `localhost:3000` in a browser to view the web application.

### Unity Installation
If you have not already, download and install [Unity Hub](https://unity.com/download). Once installed, run Unity Hub and then, on the `Projects` page, select the `Open` button. Navigate to the location of this project and choose the `unity-app/Algorithmic Art` folder. If you have not already installed the version of Unity this project was created in, there will be a warning next to the project in Unity Hub. You can simply try to open the project and Unity will ask you if you would like to install the corresponding editor version.

Once the project is loaded into Unity Hub and the right editor version is installed, the Unity portion of this project should be set up.

---
## Notes on Technologies
### React-Unity Communication
This project uses the `react-unity-webgl` module to facilitate communication between the Unity application and the React application. Since the Unity app will be embedded in the React app, there needs to be a way to send signals/content between Unity and the frontend.

In the sample code, this communication works both ways. Adjusting the slider on the frontend, found below the Unity app, will send the value of the slider to Unity to be used as the rotation speed of the cubes. Clicking the camera icon within the Unity app will take a screenshot and send the image to React encoded as a base64 string. The images that have been sent to the frontend can be found by scrolling down the page (you have to reload the page first).

A basic introduction to the `react-unity-webgl` module can be found [here](https://react-unity-webgl.dev/docs/getting-started/hello-world). For more information on how the communication between React and Unity works, see these docs: [React-to-Unity](https://react-unity-webgl.dev/docs/api/send-message) and [Unity-to-React](https://react-unity-webgl.dev/docs/api/event-system). In addition to this documentation, the sample code is documented in detail when pertaining to communication.

The React-to-Unity example provided in this code is not particularly applicable to the goals of the project, but it does showcase how to send a message from the frontend to Unity. However, the Unity-to-React communication included in this sample code will be useful in your implementation as sending resulting algorithmic art to the frontend as an image will be critical to saving and displaying images. Feel free to use this existing code in your implementation to send images from Unity to React.

The part of this communication process that you will likely have to adjust is the method by which Unity takes a picture of the scene. In this sample code, the Unity script responsible for taking the picture to send to the frontend uses the [ScreenCapture.CaptureScreenshot](https://docs.unity3d.com/ScriptReference/ScreenCapture.CaptureScreenshot.html) method, which will capture everything the camera sees. Since your project will involve a larger UI that the resulting image to send to the frontend is only a part of, a different method of "taking a picture" of a part of your Unity scene will need to be implemented.

### Embedding a Unity App
Unfortunately, the Unity app is embedded into the React app as a build, meaning that you must build the Unity app each time that you want to test it in conjunction with the frontend. To build the Unity app, select `File>Build Settings`. Make sure that the selected platform to build for is **WebGL**. Don't worry about any of the other options. You can go ahead and build the app, and when prompted for a folder, either create or select a folder in the root of the project called **Build**. The build process could take a few minutes depending on whether it is your first time building the project or not.

Once built, you need to move the files representing the WebGL version of the app into a place where the React app can access them. If you used the **Build** folder for your build, these files will be found in the `Build/Build` folder. In this folder, you will find 4 files:
```
- Build.data.br
- Build.framework.js.br
- Build.loader.js
- Build.wasm.br
```
Go ahead and copy these 4 files. Note that 3 of the files are compressed (`.br`) and will have to be decompressed once moved to their final location. In the sample code, we have specified the `frontend/public/build` folder on the web app side to be the location for the build. Move the build files to this folder.

The `.br` file extension is for the [Brotli](https://github.com/google/brotli) compression algorithm. To unpack these files, you can install the `brotli` command on linux by running `sudo apt i brotli` and then running `brotli -d <file-name>` on each file. Once decompressed, these build files are good to go.

Now that your most recent build is accessible by the React app, the embedded Unity app should reload and display all of your changes.
