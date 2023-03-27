/* eslint-disable react-hooks/exhaustive-deps */
import React, { createContext, useMemo, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import Alert from "@mui/material/Alert";
import Snackbar from "@mui/material/Snackbar";

const ApiContext = createContext(null);

function ApiProvider({ children }) {
  const [errorMessage, setErrorMessage] = useState("");

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

  return (
    <ApiContext.Provider value={useMemo(() => ({ fetchData }), [fetchData])}>
      <Snackbar open={!!errorMessage}>
        <Alert severity="error" sx={{ width: "100%" }}>
          {errorMessage}
        </Alert>
      </Snackbar>
      {children}
    </ApiContext.Provider>
  );
}

export { ApiContext, ApiProvider };
