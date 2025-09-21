sequenceDiagram
    participant FE as Frontend
    participant BE as Backend (SP)
    participant IDP as Keycloak (IdP)

    FE->>BE: GET /login
    BE->>IDP: Redirect to /authorize
    IDP->>FE: Login page
    FE->>IDP: Submit credentials
    IDP->>BE: Redirect /callback with code
    BE->>IDP: POST /token (exchange code)
    IDP-->>BE: Return access + id token
    BE->>BE: Map tokens to custom session/token
    BE-->>FE: Return custom token


