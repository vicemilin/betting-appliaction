/* eslint-disable react-hooks/exhaustive-deps */
import React, { createContext, useMemo, useState } from "react";
import axios from "axios";
import Alert from "@mui/material/Alert";
import Snackbar from "@mui/material/Snackbar";

const ApiContext = createContext(null);

function ApiProvider({ children }) {
  const [errorMessage, setErrorMessage] = useState("");
  const [infoMessage, setInfoMessage] = useState("");

  const getErrorMessage = (err) => {
    const errorMessage = err.response?.data?.message;
    const error = errorMessage ? errorMessage : "Something went wrong...";
    return error;
  };

  const fetchData = async function (reqMethod, path, reqData) {
    try {
      let headers = {};
      const response = await axios({
        method: reqMethod,
        url: `${process.env.REACT_APP_SERVER_URL}${path}`,
        data: reqData,
        headers: headers,
      });
      return response;
    } catch (err) {
      const error = getErrorMessage(err);
      setErrorMessage(error);
      setTimeout(() => setErrorMessage(null), 7000);
    }
  };

  const showErrorMessage = (message) => {
    setErrorMessage(message);
    setTimeout(() => setErrorMessage(null), 7000);
  }

  const showInfoMessage = (message) => {
    setInfoMessage(message);
    setTimeout(() => setInfoMessage(null), 7000);
  }

  return (
    <ApiContext.Provider value={useMemo(() => ({ fetchData, showErrorMessage, showInfoMessage }), [fetchData, showErrorMessage, showInfoMessage])}>
      <Snackbar open={!!errorMessage}>
        <Alert severity="error" sx={{ width: "100%" }}>
          {errorMessage}
        </Alert>
      </Snackbar>
      <Snackbar open={!!infoMessage}>
        <Alert severity="info" sx={{ width: "100%" }}>
          {infoMessage}
        </Alert>
      </Snackbar>
      {children}
    </ApiContext.Provider>
  );
}

export { ApiContext, ApiProvider };
