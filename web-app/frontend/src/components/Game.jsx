import React, { useState, useCallback, useEffect } from "react";
import { Unity, useUnityContext } from "react-unity-webgl";

// Path from the "public" folder to the location of the Unity game build.
// In this case, the build is contained in the "/public/build" folder.
const unityBuildLocation = "build";

// Component which contains the embedded Unity game.
function Game() {
    // Set up the state of our component.
    const [sliderValue, setSliderValue] = useState(50);
    const [base64Image, setBase64Image] = useState();

    // URL of the web app backend.
    // update backend URL for each run
    const backendURL = "http://localhost:8000";

    // Set up the state of the Unity application by specifying the location of the build files.
    //
    // See: https://react-unity-webgl.dev/docs/main-concepts/unity-config
    const { unityProvider, sendMessage, addEventListener, removeEventListener } = useUnityContext({
        loaderUrl: `${unityBuildLocation}/Build.loader.js`,
        dataUrl: `${unityBuildLocation}/Build.data`,
        frameworkUrl: `${unityBuildLocation}/Build.framework.js`,
        codeUrl: `${unityBuildLocation}/Build.wasm`,
    });

    // Saves an image encoded as a base64 string to the database.
    async function saveImage(encodedImage) {
        let body = {
            "encoded_image": encodedImage,
        }
        let response = await fetch(`${backendURL}/api/images`, {
            method: "POST",
            mode: "cors",
            body: JSON.stringify(body),
            headers: {
                "Content-Type": "application/json",
                "Bypass-Tunnel-Reminder": '',
            }
        })
    }

    // Handles slider value updates.
    const handleSliderChange = (event) => {
        // Update slider value.
        setSliderValue(event.target.value);

        // Round the slider value so that it is an integer.
        let roundedValue = Math.round(sliderValue);

        // This sends a message to the Unity game, telling it to find the game object called "Cube 1"
        // and then execute the function "updateRotationSpeed", passing along roundedValue as a parameter.
        // This, of course, assumes that the named function actually exists as a part of some script
        // attached to the specified game object.
        // Also, it's important to check that the type of the parameter here and in the Unity script are
        // the same. This is why we convert the slider value to an integer, since the value this function
        // expects is an integer.
        // If the types do not match, you will get an error that says that the function cannot be located.
        //
        // See: https://react-unity-webgl.dev/docs/api/send-message
        sendMessage("Cube 1", "updateRotationSpeed", roundedValue); 

        // Update the rotation speed of the rest of the cubes. (Just separated for comment readability.)
        // Cubes are named "Cube N" where N is a number from 1 to 5.
        for (let i = 2; i <= 5; i++) {
        // This sends a message to the Unity game, telling it to find the game object called "Cube"
        sendMessage("Cube " + i, "updateRotationSpeed", Math.round(sliderValue)); 
        }
    };

    // Callback function for the event listener bound to the SendImage event.
    // Runs whenever the browser receives an image from the Unity game.
    //
    // We call saveImage() to upload the image to the database after updating
    // the state.
    const handleGetImage = useCallback((base64Image) => {
        // Update base64 string that represents the image (in case you wanted to do something with
        // it besides saving to database).
        setBase64Image(base64Image);

        // Save the image to the database.
        saveImage(base64Image);
    }, []);

    // On component mount, add a Unity event listener for calls to the SendImage() function.
    // This binds the event listener callback to the handleGetImage() function.
    // You can also bind the callback to a state instead of a function, an example of which
    // can be found on the website below. We don't want to bind to state in this case because
    // we need to perform multiple actions when we receive an image.
    //
    // See: https://react-unity-webgl.dev/docs/api/event-system#registering-event-listeners
    useEffect(() => {
        addEventListener("SendImage", handleGetImage);
        return () => {
        removeEventListener("SendImage", handleGetImage);
        };
    }, [addEventListener, removeEventListener, handleGetImage]);

    // JSX content that will be rendered in the browser.
    return (
        <>
            <div id="game" style={{ flexGrow: "1" }}>
                <Unity unityProvider={unityProvider} style={{ width: "100%", height: "100%" }} />
            </div>
            {/* <div>
                <h3>Adjust cube rotation speed:</h3>
                <input type="range" min="-100" max="100" value={sliderValue} onChange={handleSliderChange} />
            </div> */}
        </>
    )
}

export default Game