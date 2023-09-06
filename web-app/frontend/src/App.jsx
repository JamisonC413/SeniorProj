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
      <h1 style={{ flexGrow: "0" }}>Sample Application</h1>
      <Game />
      <SavedImages style={{ flexGrow: "0" }}/>
    </div>
  );
}

export default App;
