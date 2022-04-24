import * as React from 'react';
import ApiClient from "../Common/ApiClient";
import {Container, Grid, TextField} from "@mui/material";
import Button from "@mui/material/Button";
import AutoFixHighRoundedIcon from "@mui/icons-material/AutoFixHighRounded";
import ContentCopyRoundedIcon from "@mui/icons-material/ContentCopyRounded";

function UrlShortener() {
    const [urlToShorten, SetUrlToShorten] = React.useState("");
    const [shortenedUrl, SetShortenedUrl] = React.useState("");

    async function shortenUrl() {
        let response = await ApiClient.CreateShortUrl(urlToShorten);
        SetShortenedUrl(response.data.shortenedUrl);
    }

    function copyUrl() {
        navigator.clipboard.writeText(shortenedUrl);
    }

    return (
        <Container maxWidth="sm">
            <Grid container rowSpacing={2} columnSpacing={{xs: 1, sm: 2, md: 3}}>
                <Grid item xs={8}>
                    <TextField className="inputFixedSize" value={urlToShorten} fullWidth={true} onChange={e => SetUrlToShorten(e.target.value)} id="urlToShorten" label="URL to shorten"
                               variant="outlined"/>
                </Grid>
                <Grid item xs={4}>
                    <Button variant="outlined" onClick={shortenUrl} className="actionButton" fullWidth={true} startIcon={<AutoFixHighRoundedIcon/>}>Shorten URL</Button>
                </Grid>
                <Grid item xs={8}>
                    <TextField className="inputFixedSize" value={shortenedUrl} fullWidth={true} disabled={true} onChange={e => SetUrlToShorten(e.target.value)} id="shortenedUrl" label="Shortened URL"
                               variant="outlined"/>
                </Grid>
                <Grid item xs={4}>
                    <Button variant="outlined" onClick={copyUrl} fullWidth={true} className="actionButton" startIcon={<ContentCopyRoundedIcon/>}>Copy</Button>
                </Grid>
            </Grid>
        </Container>
    );
}

export default UrlShortener;