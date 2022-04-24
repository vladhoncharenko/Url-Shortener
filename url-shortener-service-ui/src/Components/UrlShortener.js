import * as React from 'react';
import ApiClient from "../Common/ApiClient";
import {Alert, Container, Grid, Snackbar, TextField} from "@mui/material";
import Button from "@mui/material/Button";
import AutoFixHighRoundedIcon from "@mui/icons-material/AutoFixHighRounded";
import ContentCopyRoundedIcon from "@mui/icons-material/ContentCopyRounded";

function UrlShortener() {
    const [isInputValid, SeIsInputValid] = React.useState(true);
    const [isErrorOpened, SeIsErrorOpened] = React.useState(false);
    const [urlToShorten, SetUrlToShorten] = React.useState("");
    const [shortenedUrl, SetShortenedUrl] = React.useState("");
    const [errorMessage, SetErrorMessage] = React.useState("");

    let errorText = "Incorrect URL, please enter URL in a format like this: https://www.google.com";

    async function shortenUrl() {
        let adjustedUrl = adjustUrl(urlToShorten);
        let isValid = isUrlValid(adjustedUrl);
        SeIsInputValid(isValid);
        if (!isValid) {
            SetErrorMessage(errorText);
            return;
        }

        SetErrorMessage("");
        try {
            let response = await ApiClient.CreateShortUrl(urlToShorten);
            SetShortenedUrl(response.data.shortenedUrl);
            SeIsErrorOpened(false);
        } catch (e) {
            SeIsErrorOpened(true);
        }
    }

    function copyUrl() {
        navigator.clipboard.writeText(shortenedUrl);
    }

    function isUrlValid(urlToShorten) {
        if (!urlToShorten || urlToShorten.indexOf(' ') >= 0)
            return false;

        return /^(?:\w+:)?\/\/([^\s\.]+\.\S{2}|localhost[\:?\d]*)\S*$/.test(urlToShorten);
    }

    function adjustUrl(urlToAdjust) {
        if (urlToAdjust.search(/^http[s]?\:\/\//) == -1) {
            urlToAdjust = 'http://' + urlToAdjust;
        }
        return urlToAdjust;
    }

    return (
        <Container maxWidth="sm">
            <Grid container rowSpacing={2} columnSpacing={{xs: 1, sm: 2, md: 3}}>
                <Grid item xs={8}>
                    <TextField className="inputFixedSize" value={urlToShorten} fullWidth={true} onChange={e => SetUrlToShorten(e.target.value)} id="urlToShorten" label="URL to shorten"
                               variant="outlined" error={!isInputValid} helperText={errorMessage}/>
                </Grid>
                <Grid item xs={4}>
                    <Button variant="outlined" onClick={shortenUrl} className="actionButton" fullWidth={true} startIcon={<AutoFixHighRoundedIcon/>}>Shorten URL</Button>
                </Grid>
                <Grid item xs={8}>
                    <TextField className="inputFixedSize" value={shortenedUrl} fullWidth={true} disabled={true} onChange={e => SetUrlToShorten(e.target.value)} id="shortenedUrl"
                               label="Shortened URL"
                               variant="outlined"/>
                </Grid>
                <Grid item xs={4}>
                    <Button variant="outlined" onClick={copyUrl} fullWidth={true} className="actionButton" startIcon={<ContentCopyRoundedIcon/>}>Copy</Button>
                </Grid>
            </Grid>
            <Snackbar open={isErrorOpened} autoHideDuration={100}>
                <Alert severity="error" sx={{width: '100%'}}>
                    Something happened, please try again :(
                </Alert>
            </Snackbar>
        </Container>
    );
}

export default UrlShortener;