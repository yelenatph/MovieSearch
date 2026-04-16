# Movie Search (Blazor)

## Description

Movie Search is a **Blazor Web App** that lets users find movies and TV titles using the **[OMDb API](https://www.omdbapi.com/)**. You can search by title, browse paginated results, revisit recent searches stored locally, and open a detail view with poster art and rich metadata (cast, plot, ratings, and more).

The UI is built with **Blazor** in **.NET 10**, using **interactive server** rendering so component logic runs on the server while the browser receives updates over a SignalR connection.

## Prerequisites

- [.NET SDK 10.0](https://dotnet.microsoft.com/download)

## How to run locally

1. **Clone or open** this repository and go to the folder that contains `MovieSearch.Web` (this document assumes the `MovieSearch` project root).

2. **Configure the OMDb API key** (required for live searches):

   - Request a free key at [omdbapi.com](https://www.omdbapi.com/apikey.aspx).
   - Set `OmdbApiServiceSettings:ApiKey` in `MovieSearch.Web/appsettings.json`
 
3. **Restore and run** the web project:

   ```bash
   dotnet restore
   dotnet run --project MovieSearch.Web
   ```

4. **Open the app** in a browser using the HTTPS URL shown in the console (for example `https://localhost:7xxx`).

5. **Run tests** (optional):

   ```bash
   dotnet test MovieSearch.Tests
   ```

## Configuration reference

| Setting | Purpose |
|--------|---------|
| `OmdbApiServiceSettings:BaseUrl` | OMDb API base URL (default: `https://www.omdbapi.com/`) |
| `OmdbApiServiceSettings:ApiKey` | Your OMDb API key |
| `ConnectionStrings:DefaultConnection` | SQLite database file for search history (default: `moviesearch.db` in the app working directory) |

## Technical notes

The app uses `AddInteractiveServerComponents()` and `AddInteractiveServerRenderMode()`—this is **interactive Blazor Server** (not WebAssembly). Pages use `@rendermode InteractiveServer` (or `InteractiveServerRenderMode` where prerendering is tuned).
- **Search history**: Stored with **Entity Framework Core** and **SQLite**. On startup, `EnsureCreated()` creates the database if it does not exist.
- **Movie details page**: `MovieDetails` can use `InteractiveServerRenderMode(prerender: false)` to avoid running data-loading logic twice (prerender + interactive).
- **Search results**: Results are loaded from OMDb and shown in **QuickGrid**.
- **Tests**: `MovieSearch.Tests` uses **NUnit** for service-layer and data-layer tests.
