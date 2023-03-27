import { useEffect, useState, useContext } from "react";

import Button from "@mui/material/Button";
import Container from "@mui/material/Container";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell, { tableCellClasses } from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import TablePagination from "@mui/material/TablePagination";
import {
  FormControl,
  FormControlLabel,
  FormLabel,
  Radio,
  RadioGroup,
} from "@mui/material";

import { makeStyles } from "@mui/styles";
import { styled } from "@mui/material/styles";
import { ApiContext } from "../context/apicontext.jsx";

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: "90vw",
  height: "90vh",
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

const useStyles = makeStyles((theme) => ({
  filters: {
    display: "flex",
    flexDirection: "row",
    width: "80vw",
    alignItems: "center",
    margin: "15px",
    justifyContent: "center",
    paddingTop: "10px",
    paddingBottom: "10px",
  },
  divs: {
    marginRight: "5px",
  },
}));

const StyledTableCell = styled(TableCell)(({ theme }) => ({
  [`&.${tableCellClasses.head}`]: {
    backgroundColor: theme.palette.secondary.light,
    color: theme.palette.common.white,
  },
}));

const StyledTableRow = styled(TableRow)(({ theme }) => ({
  "&:nth-of-type(odd)": {
    backgroundColor: theme.palette.action.hover,
  },
}));

const BettingPage = () => {
  const [offerState, setOfferState] = useState(null);
  const apiContext = useContext(ApiContext);

  const fetchOffer = async () => {
    const result = await apiContext.fetchData("get", "/Offer");
    console.log(result);
    setOfferState(result.data);
  };

  useEffect(() => {
    fetchOffer();
  }, []);

  return offerState ? (
    <>
      <Table size="small" sx={{ marginTop: "50px" }}>
        <TableHead>
          <TableRow>
            <StyledTableCell>Match</StyledTableCell>
            <StyledTableCell>Start Time</StyledTableCell>
            <StyledTableCell>1</StyledTableCell>
            <StyledTableCell>X</StyledTableCell>
            <StyledTableCell>2</StyledTableCell>
            <StyledTableCell>1X</StyledTableCell>
            <StyledTableCell>X2</StyledTableCell>
            <StyledTableCell>12</StyledTableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {offerState.map((event) => (
            <StyledTableRow key={event.id}>
              <StyledTableCell>{`${event.homeTeam} - ${event.awayTeam}`}</StyledTableCell>
              <StyledTableCell>
                {new Date(event.startTime).toLocaleTimeString()}
              </StyledTableCell>
            </StyledTableRow>
          ))}
        </TableBody>
      </Table>
    </>
  ) : null;
};

export default BettingPage;
