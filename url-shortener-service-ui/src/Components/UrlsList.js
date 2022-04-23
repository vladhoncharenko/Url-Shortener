import * as React from 'react';
import Pagination from '@mui/material/Pagination';
import PaginationItem from '@mui/material/PaginationItem';
import Stack from '@mui/material/Stack';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import ApiClient from "../Common/ApiClient";
import {Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography} from "@mui/material";

function UrlsList() {
    const itemsPerPage = 5;
    const [page, SetPage] = React.useState(1);
    const [pagesAmount, SetPagesAmount] = React.useState(1);
    const [urlsData, SetUrlsData] = React.useState([]);

    async function handlePageChange(event, newPage) {
        SetPage(newPage);
        await getShortenedUrls(newPage, itemsPerPage);
    }

    function SetAvailablePages(countOfAllItems) {
        if (countOfAllItems > itemsPerPage)
            SetPagesAmount(Math.ceil((countOfAllItems - itemsPerPage) / itemsPerPage) + 1);
        else
            SetPagesAmount(1);
    }

    async function getShortenedUrls(page, pageCapacity) {
        const response = await ApiClient.GetShortUrl(page, pageCapacity);
        SetAvailablePages(response.data.Count);
        SetUrlsData(response.data.Data)
    }

    React.useEffect(() => {
        async function fetchData() {
            return getShortenedUrls(1, itemsPerPage);
        }
        fetchData();
    }, []);

    return (<Stack spacing={2}>
        <TableContainer component={Paper}>
            <Table aria-label="simple table">
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
                            <TableCell component="th" scope="row">{urlData.ShortenedUrl}</TableCell>
                            <TableCell align="left">{urlData.RedirectsCount}</TableCell>
                            <TableCell align="left">{urlData.CreatedOn}</TableCell>
                            <TableCell align="left">{urlData.LastRedirect}</TableCell>
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
    </Stack>);
}

export default UrlsList;