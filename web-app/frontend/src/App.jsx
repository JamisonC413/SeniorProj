import './App.css';
import Game from './components/Game';
import ImageGallery from "./components/ImageGallery"; 
import SavedImages from './components/SavedImages';
import NavBar from './components/NavBar';
import { BrowserRouter as Router, Link, Routes, Route } from "react-router-dom"; 


function App() {
  return (
    <Router>
    <NavBar />
    <div className="App"
      style={{
        maxHeight: "100vh",
        display: "flex",
        flexDirection: "column",
        padding: "15px"
      }}>
        <Routes>
          <Route exact path="/" element={<Game />} />
          <Route path="/imageGallery" element={<ImageGallery />} />
        </Routes>
    </div>
    </Router>

  );
}

export default App;
