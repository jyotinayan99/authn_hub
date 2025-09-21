## Basic Auth Flow
1. User submits username/password to `/login`
2. Backend validates credentials against DB
3. Backend creates session + custom token
4. Backend returns token to frontend

## SSO / OIDC Flow
1. Frontend requests `/login` → backend redirects to Keycloak `/authorize`
2. User authenticates at Keycloak
3. Keycloak redirects to backend `/callback` with code
4. Backend exchanges code for tokens at `/token` endpoint
5. Backend maps Keycloak token → custom token
6. Backend returns token to frontend
