import React from "react";
import { useToken } from "../contexts/TokenContext";

export default function Dashboard() {
  const { accessToken, refreshToken, clear } = useToken();

  return (
    <div>
      <h2>Dashboard</h2>
      <div><strong>Access token:</strong></div>
      <textarea style={{width: "100%", height: 120}} readOnly value={accessToken || "(none)"} />
      <div><strong>Refresh token:</strong></div>
      <textarea style={{width: "100%", height: 80}} readOnly value={refreshToken || "(none)"} />
      <div style={{marginTop:12}}>
        <button onClick={() => { clear(); window.location = "/"; }}>Logout</button>
      </div>
    </div>
  );
}
