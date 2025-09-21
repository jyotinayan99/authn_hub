import React, { createContext, useContext, useState, useEffect } from "react";

const TokenContext = createContext();

export function TokenProvider({ children }) {
  const [accessToken, setAccessToken] = useState(() => localStorage.getItem("accessToken"));
  const [refreshToken, setRefreshToken] = useState(() => localStorage.getItem("refreshToken"));

  useEffect(() => {
    if (accessToken) localStorage.setItem("accessToken", accessToken);
    else localStorage.removeItem("accessToken");
  }, [accessToken]);

  useEffect(() => {
    if (refreshToken) localStorage.setItem("refreshToken", refreshToken);
    else localStorage.removeItem("refreshToken");
  }, [refreshToken]);

  const clear = () => {
    setAccessToken(null);
    setRefreshToken(null);
  };

  return (
    <TokenContext.Provider value={{ accessToken, setAccessToken, refreshToken, setRefreshToken, clear }}>
      {children}
    </TokenContext.Provider>
  );
}

export function useToken() {
  return useContext(TokenContext);
}
