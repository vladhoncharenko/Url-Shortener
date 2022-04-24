import * as React from 'react';
import Pagination from '@mui/material/Pagination';
import PaginationItem from '@mui/material/PaginationItem';
import Stack from '@mui/material/Stack';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import ApiClient from "../Common/ApiClient";
import {Container, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography} from "@mui/material";
import moment from "moment";

function UrlsList() {
    const itemsPerPage = 5;
    const [page, SetPage] = React.useState(1);
    const [pagesAmount, SetPagesAmount] = React.useState(1);
    const [urlsData, SetUrlsData] = React.useState([]);

    async function handlePageChange(event, newPage) {
        SetPage(newPage);
        await getShortenedUrls(newPage, itemsPerPage);
    }

    function setAvailablePages(countOfAllItems) {
        if (countOfAllItems > itemsPerPage)
            SetPagesAmount(Math.ceil((countOfAllItems - itemsPerPage) / itemsPerPage) + 1);
        else
            SetPagesAmount(1);
    }

    function formatDate(dateToFormat) {
        if (dateToFormat == null)
            return "No Redirects"
        else
            return moment(dateToFormat).format('MM/DD/YYYY h:mm A')
    }

    function formatUrl(urlToFormat) {
        return urlToFormat.split("/").pop();
    }

    async function getShortenedUrls(page, pageCapacity) {
        const response = await ApiClient.GetShortUrl(page, pageCapacity);
        setAvailablePages(response.data.Count);
        SetUrlsData(response.data.Data)
    }

    React.useEffect(() => {
        async function fetchData() {
            return getShortenedUrls(1, itemsPerPage);
        }

        fetchData();
    }, []);

    return (
        <Container maxWidth="sm">
            <Stack spacing={2} justifyContent="center" alignItems="center">
                <TableContainer component={Paper}>
                    <Table aria-label="simple table" className="urlsTable">
                        <TableHead>
                            <TableRow>
                                <TableCell align="left">Short URL</TableCell>
                                <TableCell align="left">Redirects</TableCell>
                                <TableCell align="left">Created At</TableCell>
                                <TableCell align="left">Last Redirect</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {urlsData.map((urlData) => (
                                <TableRow key={urlData.ShortenedUrl} sx={{'&:last-child td, &:last-child th': {border: 0}}}>
                                    <TableCell component="th" scope="row">
                                        <a href={urlData.ShortenedUrl}>{formatUrl(urlData.ShortenedUrl)}</a>
                                    </TableCell>
                                    <TableCell align="left">{urlData.RedirectsCount}</TableCell>
                                    <TableCell align="left">{formatDate(urlData.CreatedOn)}</TableCell>
                                    <TableCell align="left">{formatDate(urlData.LastRedirect)}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
                <Pagination
                    count={pagesAmount}
                    renderItem={(item) => (
                        <PaginationItem components={{previous: ArrowBackIcon, next: ArrowForwardIcon}} {...item} />
                    )}
                    page={page}
                    onChange={handlePageChange}
                />
            </Stack>
        </Container>);
}

export default UrlsList;