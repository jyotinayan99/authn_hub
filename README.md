# authn_hub
Unified Authentication System — SSO + Local Auth + Custom Token Management

🚀 Overview
authn_hub is a reference implementation of a full authentication system for modern web applications. 
It supports:
	OIDC-based Single Sign-On (SSO) via Keycloak, including federated IdPs.
	Local authentication (username/password).
	Custom token issuance (JWT/opaque) for frontend APIs.
	Session management and optional token refresh.
	Extensible architecture with separate backend, frontend, and configuration layers.

This repo is designed for developers who want a starter authentication system with enterprise-grade patterns and a realistic flow.

⚙️ Tech Stack
	Layer	Technology
	Identity Provider	Keycloak (standalone or federated)
	Backend / Service Provider	.NET 8 Web API
	Frontend	React.js (functional components + hooks)
	Database	EF Core (SQL Server / PostgreSQL optional)
	Documentation	Mermaid diagrams + Markdown

```
📁 Repository Structure
/authn_hub
│
├── /backend
│    ├── /Server          # Main .NET Web API
│    ├── /Config          # Keycloak & JWT configuration
│    ├── /Models          # Domain models and DTOs
│    ├── /Services        # Business logic
│    ├── /Repository      # DB access layer
│    ├── /Data            # EF Core DbContext & migrations
│    ├── /Utils           # Helpers (OIDC validation, encryption)
│    └── /Tests           # Unit and integration tests
│
├── /frontend
│    ├── /src
│    │    ├── /components # React components (LoginButton, Dashboard, TokenContext)
│    │    ├── /pages      # Pages (Home, Callback)
│    │    └── /services   # API helpers for backend communication
│    ├── package.json
│    └── /tests           # React unit tests
│
├── /docs
│    ├── flow.md           # Step-by-step authentication flow
│    ├── diagrams.mmd      # Mermaid diagrams (happy path, architecture)
│    └── architecture.md   # High-level architecture overview
│
├── .gitignore
└── README.md
```

🔹 Features
Unified login endpoints
	/login → initiates OIDC / local auth.
	/callback → handles OIDC authorization code exchange.
	/token/refresh → optionally refresh custom tokens.

Custom token management
	Backend issues custom JWT/opaque tokens mapped to IdP claims.
	Frontend only sees SP-issued tokens, hiding internal IdP tokens.
	Flexible user management
	Maps external IdP users to local database accounts.
	Supports multiple IdPs for federated login.

Frontend integration
	React app uses TokenContext for storing custom token.
	LoginButton redirects to backend /login.
	Callback page handles redirect with authorization code.
	Extensible & modular
	Clear separation: backend, frontend, config, models, services, repo.
	Easy to extend for more auth features (MFA, role-based access, OAuth APIs).

🔹 Getting Started (Development)

Note: This version does not include Docker; backend and frontend run independently.
1. Setup Keycloak
	Install Keycloak locally: https://www.keycloak.org/downloads
	Import realm-export.json from /backend/Config or /keycloak.
	Create a client for the backend API and configure redirect URIs.

2. Run Backend
	cd backend/Server
	dotnet restore
	dotnet run

3. Run Frontend
	cd frontend
	npm install
	npm start

4. Access Application
Open browser: http://localhost:3000 (React app)
Click Login → redirect to Keycloak SSO → returns custom token

🔹 Documentation
	Flow diagrams: /docs/diagrams.mmd (Mermaid)
	Step-by-step flow: /docs/flow.md
	Architecture overview: /docs/architecture.md
🔹 License
  This project is licensed under the Apache License 2.0 — see the LICENSE file for details.
🔹 Contributing
	Contributions are welcome! Suggested contributions:
	Adding multi-factor authentication (MFA)
	Adding support for more federated IdPs
	Improving frontend UI/UX
	Unit & integration test coverage
