# Mission #10 (IS 413) — Step-by-step Guide

Source: `IS413 - Mission10Assignment.pdf`

## 0) Goal (what you’re building)

Build a React website that lists bowlers from the Bowling League database by calling your ASP.NET API.

Your UI must display **only** bowlers on the **Marlins** or **Sharks** teams, and show:

- Bowler Name (First, Middle, Last)
- Team Name
- Address
- City
- State
- Zip
- Phone Number

You will create:

- A **Heading** component that describes what the page is about
- A **Table** component that lists the bowlers
- Use both components in `App`

Submission: **Public GitHub repo link** (Learning Suite).

---

## 1) Download the database

1. Download the database from the link in the PDF:
   - `https://byu.box.com/s/4o6737azrchv04desaajncibesywbpgis`
2. Unzip it (if needed).
3. Identify what you got (this determines your setup steps):
   - **SQL Server backup**: `.bak`
   - **SQL Server database file**: `.mdf` (sometimes with a `.ldf`)
   - **SQLite**: `.db` or `.sqlite`
   - **Script**: `.sql` (create tables + insert data)

Keep the database file(s) somewhere safe (some instructors want it *not* committed to GitHub).

---

## 2) Backend (ASP.NET API) — create an endpoint for bowlers

Your backend lives in `backend/Mission10_LaTour`.

### 2.1 Add EF Core packages (one-time)

In `backend/Mission10_LaTour`, add EF Core packages for *your database type*:

- SQL Server:
  - `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`
  - `dotnet add package Microsoft.EntityFrameworkCore.Tools`
- SQLite:
  - `dotnet add package Microsoft.EntityFrameworkCore.Sqlite`
  - `dotnet add package Microsoft.EntityFrameworkCore.Tools`

### 2.2 Add a connection string

In `backend/Mission10_LaTour/appsettings.json`, add a `ConnectionStrings` section.

Examples (pick one that matches your DB):

- SQL Server (example):
  - `Server=localhost;Database=BowlingLeague;User Id=...;Password=...;TrustServerCertificate=True;`
- SQLite (example):
  - `Data Source=../BowlingLeague.db`

### 2.3 Create EF models + DbContext

Recommended: **scaffold** models from the database so table/column names match exactly.

From `backend/Mission10_LaTour`:

- SQL Server (example pattern):
  - `dotnet ef dbcontext scaffold "YOUR_CONNECTION_STRING" Microsoft.EntityFrameworkCore.SqlServer -o Models -c BowlingLeagueContext -f`
- SQLite:
  - `dotnet ef dbcontext scaffold "Data Source=PATH_TO_DB" Microsoft.EntityFrameworkCore.Sqlite -o Models -c BowlingLeagueContext -f`

After scaffolding, confirm you have:

- `Models/Bowler.cs` (or similar)
- `Models/Team.cs` (or similar)
- `Models/BowlingLeagueContext.cs` (or your chosen context name)

### 2.4 Register DbContext in `Program.cs`

In `backend/Mission10_LaTour/Program.cs`:

1. Add the DbContext to services (using your provider):
   - SQL Server: `UseSqlServer(...)`
   - SQLite: `UseSqlite(...)`
2. Add CORS so your React app can call the API during development.

Typical CORS setup:

- Allow requests from your Vite dev server (commonly `http://localhost:5173`)

### 2.5 Create a DTO for the API response

Create a DTO so you only return the fields required by the assignment and shape them clearly.

Suggested DTO fields:

- `firstName`, `middleInit`, `lastName`
- `teamName`
- `address`, `city`, `state`, `zip`
- `phoneNumber`

### 2.6 Create the Bowlers controller + endpoint

Create a controller (recommended route):

- `GET /api/bowlers`

Requirements for the query:

1. Join bowlers to teams so you can return **Team Name**
2. Filter to **only** `TeamName == "Marlins"` or `TeamName == "Sharks"`
3. Return the fields needed for the table

Notes:

- Filtering in the **API** is preferred (less data sent to the client).
- Use `AsNoTracking()` for read-only queries.

### 2.7 Quick backend test

Run the backend and hit your endpoint:

- Start: `dotnet run`
- Test in browser / http file / Postman:
  - `https://localhost:XXXX/api/bowlers`

You should see JSON where every record’s team is only Marlins or Sharks.

---

## 3) Frontend (React) — fetch and display bowlers in a table

Your frontend lives in `frontend/` (Vite + React + TypeScript).

### 3.1 Decide how the frontend will reach the API

Pick one:

1. **CORS approach** (simplest): React calls `https://localhost:XXXX/api/bowlers`
2. **Vite proxy approach**: configure a proxy in `frontend/vite.config.ts` so the frontend calls `/api/bowlers`

If you use the CORS approach, consider adding a `frontend/.env`:

- `VITE_API_URL=https://localhost:XXXX`

Then your fetch uses: `import.meta.env.VITE_API_URL`.

### 3.2 Create a TypeScript type for a Bowler row

Add something like `frontend/src/types/Bowler.ts` defining the fields your API returns.

### 3.3 Create the Heading component

Add `frontend/src/components/Heading.tsx` that renders:

- Page title (e.g., “Bowling League Bowlers”)
- Short subtitle (e.g., “Marlins & Sharks only”)

### 3.4 Create the Table component

Add `frontend/src/components/BowlerTable.tsx` that:

1. Fetches bowler data from the API on load (`useEffect`)
2. Stores it in state (`useState`)
3. Renders a `<table>` with columns for the required fields

Minimum column set (matches the assignment):

- Bowler Name
- Team
- Address
- City
- State
- Zip
- Phone

### 3.5 Wire components into `App.tsx`

In `frontend/src/App.tsx`:

- Render `<Heading />`
- Render `<BowlerTable />`

---

## 4) “Only Marlins or Sharks” checklist

Confirm the filter is enforced:

- Backend: the endpoint returns **only** those teams (recommended)
- Frontend: do not re-add excluded teams accidentally

---

## 5) Run it locally (dev)

In two terminals:

Backend:

- `cd backend/Mission10_LaTour`
- `dotnet run`

Frontend:

- `cd frontend`
- `npm install`
- `npm run dev`

Load the Vite URL (usually `http://localhost:5173`) and verify:

- Page has a clear heading
- Table renders bowlers
- Teams are only Marlins/Sharks
- All required columns appear

---

## 6) Submission checklist (public GitHub link)

- Repo is public
- React app runs without missing files
- API runs and returns correct data
- UI displays the required fields
- Only Marlins/Sharks shown
- Link submitted in Learning Suite

---

## Appendix: Common pitfalls

- **CORS errors**: fix in `backend/Mission10_LaTour/Program.cs` (allow your frontend origin).
- **HTTPS mismatch**: if backend is HTTPS-only, call the HTTPS URL from React.
- **Wrong DB provider**: ensure your EF Core provider matches the database file/type you downloaded.
- **Wrong table/column names**: scaffolding from the DB avoids mismatches.

