import "./bettingPage.css";
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
import { v4 as createUUID } from "uuid";

const allPicks = ["1", "x", "2", "1x", "x2", "12"];

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
  const [betslip, setBetslip] = useState([]);
  const [payinAmount, setPayinAmount] = useState(0);
  const [winAmount, setWinAmount] = useState(0);
  const [transactionId, setTransactionId] = useState(createUUID());
  const [balance, setBalance] = useState(0);
  const apiContext = useContext(ApiContext);

  const fetchOffer = async () => {
    const result = await apiContext.fetchData("get", "/Offer");
    console.log(result);
    setOfferState(result.data);
  };

  useEffect(() => {
    fetchOffer();
    refreshBalance();
  }, []);

  const addBet = (event, pick) => {
    if (betslip.some((y) => y.pick.id === pick.id)) {
      const newBetslip = [...betslip].filter((x) => x.event.id !== event.id);
      setBetslip(newBetslip);
      calculateMaxWin(payinAmount, newBetslip);
      return;
    }
    const newBetslip = [...betslip].filter((x) => x.event.id !== event.id);
    newBetslip.push({ event, pick });
    setBetslip(newBetslip);
    calculateMaxWin(payinAmount, newBetslip);
  };

  const getPicksForTable = (event) => {
    const result = allPicks.map((x) => {
      const pick = event.picks.find((y) => y.pickName === x);
      if (!pick) {
        return <StyledTableCell key={`${event.id}_${x}`} />;
      }
      return (
        <StyledTableCell
          key={`${event.id}_${x}`}
          style={{
            backgroundColor: betslip.some((y) => y.pick.id === pick.id)
              ? "green"
              : "transparent",
          }}
          onClick={() => addBet(event, pick)}
        >
          {pick.odds}
        </StyledTableCell>
      );
    });

    return result;
  };

  const calculateMaxWin = (payin, bslip) => {
    const maxWin =
      Math.round(
        bslip.reduce((acc, element) => acc * element.pick.odds, payin) * 100
      ) / 100;
    setWinAmount(maxWin);
  };

  const onPayinValueChanged = (e) => {
    e.preventDefault();
    setPayinAmount(Number(e.target.value));
    calculateMaxWin(Number(e.target.value), betslip);
  };

  const payinTicket = async () => {
    if (!betslip.length || !payinAmount) {
      apiContext.showErrorMessage("Invalid Request");
      return;
    }
    const payinRequest = {
      transactionId: transactionId,
      payinAmount: payinAmount,
      ticketBets: betslip.map((x) => ({
        pickId: x.pick.id,
        odds: x.pick.odds,
      })),
    };

    const result = await apiContext.fetchData(
      "post",
      "/Ticket/Payin",
      payinRequest
    );
    if (result.data.success) {
      apiContext.showInfoMessage("Ticket Paid In Successfully");
      setBetslip([]);
      setPayinAmount(0);
      setWinAmount(0);
      setTransactionId(createUUID());
      refreshBalance();
    } else {
      apiContext.showErrorMessage(result.data.message);
    }
  };

  const refreshBalance = async () => {
    const result = await apiContext.fetchData("get", "/Wallet/GetState");
    setBalance(result.data.currentState);
  };

  return offerState ? (
    <div className="betting-page-main-container">
      <Table
        size="small"
        sx={{ marginTop: "50px" }}
        className="betting-page-offer"
      >
        <TableHead>
          <TableRow>
            <StyledTableCell>Match</StyledTableCell>
            <StyledTableCell>Sport</StyledTableCell>
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
              <StyledTableCell>{event.sport.name}</StyledTableCell>
              <StyledTableCell>
                {new Date(event.startTime).toLocaleTimeString()}
              </StyledTableCell>
              <>{getPicksForTable(event)}</>
            </StyledTableRow>
          ))}
        </TableBody>
      </Table>
      <div className="betting-page-betslip">
        <div className="balance-container">Balance: {balance}</div>
        {betslip.map((x) => (
          <div className="betting-page-betslip-element" key={x.pick.id}>
            <h4>{`${x.event.homeTeam} - ${x.event.awayTeam}`}</h4>
            <p>{new Date(x.event.startTime).toLocaleTimeString()}</p>
            <p>
              <b>{x.pick.pickName}</b>
              {"\u00A0"}
              {x.pick.odds}
            </p>
          </div>
        ))}
        <div className="betting-page-betslip-inputs-container">
          <input
            type="number"
            value={payinAmount}
            onChange={onPayinValueChanged}
          />
          <div>Possible Win: {winAmount}</div>
          <button onClick={payinTicket}>Pay In</button>
        </div>
      </div>
    </div>
  ) : null;
};

export default BettingPage;
