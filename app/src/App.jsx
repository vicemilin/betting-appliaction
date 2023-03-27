import logo from "./logo.svg";
import "./App.css";
import { ApiProvider } from "./context/apicontext";
import BettingPage from "./pages/bettingPage";

function App() {
  return (
    <div className="App">
      <ApiProvider>
        <BettingPage />
      </ApiProvider>
    </div>
  );
}

export default App;
