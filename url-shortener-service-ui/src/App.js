import './App.css';
import UrlShortener from './Components/UrlShortener.js';
import UrlsList from './Components/UrlsList.js';
import {Typography} from "@mui/material";
import {ThemeProvider, createTheme} from '@mui/material/styles';
import Stack from "@mui/material/Stack";


function App() {
    const darkTheme = createTheme({
        palette: {
            mode: 'dark',
        },
    });

    return (
        <div className="App">
            <ThemeProvider theme={darkTheme}>
                <Stack spacing={2} justifyContent="center" alignItems="center" width={'100%'}>
                    <Typography className="headerText" variant="h2">URL Shortener</Typography>
                    <UrlShortener/>
                    <UrlsList/>
                </Stack>
            </ThemeProvider>
        </div>
    );
}

export default App;
