mergeInto(LibraryManager.library, {
    SendImage: function (base64Image) {
        // Important to wrap the event dispatch in a try/catch to ensure that errors aren't thrown
        // outside of the React Unity WebGL module.
        // See: https://react-unity-webgl.dev/docs/api/event-system#dispatching-events
        try {
            window.dispatchReactUnityEvent("SendImage", UTF8ToString(base64Image));
        } catch (e) {
            console.warn("Failed to dispatch event!");
        }
    },
});