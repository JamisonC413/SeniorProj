import React from "react"; 
import SavedImages from './SavedImages';
  
// Contains the SavedImages component
const ImageGallery = () => { 
  return ( 
    <div> 
      <SavedImages style={{ flexGrow: "0" }}/>
    </div> 
  ); 
}; 
  
export default ImageGallery; 