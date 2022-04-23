import * as React from 'react';
import ApiClient from "../Common/ApiClient";
import {Box, Grid, TextField} from "@mui/material";
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
        <Box sx={{width: '40%', height: '20%'}}>
            <Grid container spacing={1}>
                <Grid item xs={5}>
                    <TextField value={urlToShorten} onChange={e => SetUrlToShorten(e.target.value)} id="urlToShorten" label="URL to shorten" variant="outlined"/>
                </Grid>
                <Grid item xs={6}>
                    <Button variant="contained" onClick={shortenUrl} startIcon={<AutoFixHighRoundedIcon/>}>Shorten URL</Button>
                </Grid>
                <Grid item xs={10}>
                    <TextField value={shortenedUrl} onChange={e => SetUrlToShorten(e.target.value)} id="shortenedUrl" label="Shortened URL" variant="outlined"/>
                </Grid>
                <Grid item xs={2}>
                    <Button variant="contained" onClick={copyUrl} startIcon={<ContentCopyRoundedIcon/>}>Copy</Button>
                </Grid>
            </Grid>
        </Box>
    );
}

export default UrlShortener;