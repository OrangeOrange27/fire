# Client & Server Setup Guide

This repository includes two parts:
- **Unity Client** — the game frontend built with Unity and Addressables.
- **.NET Server** — a lightweight backend using ASP.NET Core Minimal API.

---

## 🔧 Prerequisites

### For the Server
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- Any code editor (Visual Studio, Rider, VS Code)
- Port `8080` must be free

---

## 🚀 How to Run the Server

1. Open a terminal in the `Server` directory (where the `.csproj` file is).
2. Run:
   ```bash
   dotnet restore
   dotnet run --urls "http://localhost:8080"
