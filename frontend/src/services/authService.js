import { apiFetch } from "./api";

/* ------------------------------
   USERNAME / PASSWORD LOGIN
------------------------------ */
export async function loginRaw(username, password) {
  return apiFetch("/auth/login/raw", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, password })
  });
}

export async function loginCustom(username, password) {
  return apiFetch("/auth/login/custom", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, password })
  });
}

/* ------------------------------
   AUTHENTICATED API CALL EXAMPLE
------------------------------ */
export async function getProfile(accessToken) {
  return fetch(`${import.meta.env.VITE_API_BASE}/users/me`, {
    headers: {
      Authorization: `Bearer ${accessToken}`
    }
  }).then(r => {
    if (!r.ok) throw new Error("Unauthorized");
    return r.json();
  });
}

/* ------------------------------
   OIDC LOGIN FLOW
------------------------------ */

/**
 * Redirect user to Keycloak login page
 * @param {"raw"|"custom"} type - determines which backend flow to call after callback
 */
export function redirectToOIDCLogin(type = "custom") {
  const kcAuthUrl = `${import.meta.env.VITE_KC_BASE}/realms/${import.meta.env.VITE_KC_REALM}/protocol/openid-connect/auth?` +
    new URLSearchParams({
      client_id: import.meta.env.VITE_KC_CLIENT,
      redirect_uri: import.meta.env.VITE_KC_REDIRECT,
      response_type: "code",
      scope: "openid profile email",
      state: type // pass "raw" or "custom" to callback
    });

  window.location.href = kcAuthUrl;
}

/**
 * Exchange OIDC authorization code for tokens via backend
 * @param {string} code - authorization code returned by Keycloak
 * @param {"raw"|"custom"} type - determines which backend endpoint to call
 */
export async function exchangeOIDCCode(code, type = "custom") {
  if (!code) throw new Error("Authorization code is required");

  // Choose backend endpoint based on type
  const endpoint = type === "custom"
    ? "/auth/oidc-exchange-custom"
    : "/auth/oidc-exchange-raw";

  try {
    return await apiFetch(endpoint, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ code })
    });
  } catch (err) {
    console.error("OIDC exchange failed:", err);
    throw err;
  }
}
