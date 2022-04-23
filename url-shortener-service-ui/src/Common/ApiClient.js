import axios from "axios";
import Config from "../config.json"

async function CreateShortUrl(originalUrl) {
    return axios.put(`${Config.SERVER_URL}/UrlShortener`,
        {
            "OriginalUrl": originalUrl
        },
        {
            headers: {
                'Content-Type': 'application/json',
            }
        });
}

async function GetShortUrl(page, pageCapacity) {
    return axios.get(`${Config.SERVER_URL}/UrlShortener/${page}/${pageCapacity}`);
}

export default {
    CreateShortUrl,
    GetShortUrl,
}