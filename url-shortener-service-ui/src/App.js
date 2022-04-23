import './App.css';
import UrlShortener from './Components/UrlShortener.js';
import UrlsList from './Components/UrlsList.js';

function App() {
    return (
        <div className="App">
            <UrlShortener/>
            <UrlsList/>
        </div>
    );
}

export default App;
