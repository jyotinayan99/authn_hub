import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { exchangeOIDCCode } from "../services/authService";
import { useToken } from "../contexts/TokenContext";

export default function Callback() {
  const { setAccessToken, setRefreshToken } = useToken();
  const navigate = useNavigate();
  const { search } = useLocation();

  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function handleOIDC() {
      const params = new URLSearchParams(search);
      const code = params.get("code");
      const state = params.get("state") || "custom"; // default to custom

      if (!code) {
        setError("No authorization code found in URL");
        setLoading(false);
        return;
      }

      try {
        const res = await exchangeOIDCCode(code, state);
        setAccessToken(res.accessToken || res.access_token);
        setRefreshToken(res.refreshToken || res.refresh_token);
        navigate("/dashboard", { replace: true });
      } catch (err) {
        setError(err.body || err.message);
      } finally {
        setLoading(false);
      }
    }
      handleOIDC();
    }, [search, setAccessToken, setRefreshToken, navigate]);

  if (error) {
    return <div style={{ color: "red" }}>OIDC Error: {String(error)}</div>;
  }

  if (loading) {
    return <div>Logging you in via Keycloak...</div>;
  }

  return null;
}
