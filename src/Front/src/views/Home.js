import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Box from '@mui/material/Box';

import useAxios from '../hooks/useAxios';


function Home() {

    const { response } = useAxios({
        url: '/api/fiis'
    });

    return (
        <Box component="main" sx={{ p: 3 }}>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell>Data Pagamento</TableCell>
                            <TableCell>Rendimento</TableCell>
                            <TableCell>DY</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {response?.map((item, index) => (
                            <TableRow key={index} >
                                <TableCell component="th" scope="row">
                                    {item.name}
                                </TableCell>
                                <TableCell>{item.date}</TableCell>
                                <TableCell>{item.value}</TableCell>
                                <TableCell>{item.dy}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    );
}

export default Home