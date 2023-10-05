import './App.css';
import Game from './components/Game';
import SavedImages from './components/SavedImages';

function App() {
  return (
    <div className="App"
      style={{
        maxHeight: "100vh",
        display: "flex",
        flexDirection: "column",
        padding: "15px"
      }}>
      <Game />
      <SavedImages style={{ flexGrow: "0" }}/>
    </div>
  );
}

export default App;
