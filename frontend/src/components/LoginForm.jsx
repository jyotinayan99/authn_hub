import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginCustom, loginRaw, redirectToOIDCLogin, redirectToFederatedOIDCLogin } from "../services/authService";
import { useToken } from "../contexts/TokenContext";

export default function LoginForm() {
  const [username, setUsername] = useState("testuser");
  const [password, setPassword] = useState("password");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const { setAccessToken, setRefreshToken } = useToken();
  const navigate = useNavigate();

  async function doCustom() {
    setLoading(true);
    setError(null);
    try {
      const res = await loginCustom(username, password);
      setAccessToken(res.accessToken);
      setRefreshToken(res.refreshToken);
      navigate("/dashboard");
    } catch (err) {
      setError(err.body || err.message);
    } finally {
      setLoading(false);
    }
  }

  async function doRaw() {
    setLoading(true);
    setError(null);
    try {
      const res = await loginRaw(username, password);
      setAccessToken(res.accessToken || res.access_token);
      setRefreshToken(res.refreshToken || res.refresh_token);
      navigate("/dashboard");
    } catch (err) {
      setError(err.body || err.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <form onSubmit={(e) => e.preventDefault()}>
      <h2>Login (POC)</h2>
      <label>Username</label>
      <input value={username} onChange={e => setUsername(e.target.value)} />
      <label>Password</label>
      <input type="password" value={password} onChange={e => setPassword(e.target.value)} />

      <div style={{ display: "flex", gap: 8, marginTop: 16, flexWrap: "wrap" }}>
        <button type="button" onClick={doCustom} disabled={loading}>Login (custom)</button>
        <button type="button" onClick={doRaw} disabled={loading}>Login (raw)</button>
        <button type="button" onClick={() => redirectToOIDCLogin("custom")}>Login OIDC (custom)</button>
        <button type="button" onClick={() => redirectToOIDCLogin("raw")}>Login OIDC (raw)</button>
        <button type="button" onClick={() => redirectToFederatedOIDCLogin("custom")}>Login via Federated IdP</button>
      </div>

      {error && <div style={{ color: "red", marginTop: 8 }}>Error: {String(error)}</div>}
    </form>
  );
}
