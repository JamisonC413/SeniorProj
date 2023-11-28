import React, { useEffect } from 'react'

// Component containing all saved images

function SavedImages() {

    // update backend URL for each run
    const backendURL = "http://localhost:8000";

    // Get and render all images
    useEffect(() => {
        const savedGallery = document.getElementById("saved-gallery");
        // Get saved images from DB
        fetch(`${backendURL}/api/images`, {
            method: "GET",
            mode: "cors",
            headers: {
                "Bypass-Tunnel-Reminder": '',
            }
        }).then(savedImages => {
            return savedImages.json();
        }).then(savedImages => {
            console.log(savedImages);
            // Empty the gallery so that the same images aren't re-added
            // during each render
            savedGallery.replaceChildren();
            for (let i = 0; i < savedImages.length; i++) {
                let galleryImage = document.createElement("img");
                galleryImage.src = "data:image/png;base64," + savedImages[i].encoded_image;
                savedGallery.appendChild(galleryImage);
            }
        })
    });

    return (
        <div>
            <h3>Image Gallery</h3>
            <div id="saved-gallery"></div>
        </div>
    )
}

export default SavedImages