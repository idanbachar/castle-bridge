# Castle Bridge

Castle Bridge is a fast-paced medieval multiplayer battle game.
Two teams fight to raid the enemy castle, steal diamonds, and return them safely.
Strategy, timing, and class synergy decide who survives the war.

<p align="center">
   <img src="./images/cover/cover.png" width="520" alt="Castle Bridge cover" />
</p>

---

## Table of Contents

1. [Game Overview](#game-overview)
2. [Core Gameplay](#core-gameplay)
3. [Playable Classes](#playable-classes)
4. [Gallery](#gallery)
5. [Installation and Setup](#installation-and-setup)
6. [Run the Game](#run-the-game)
7. [Project Structure](#project-structure)

---

## Game Overview

Castle Bridge is built around team-vs-team objective combat:

- Team Red vs Team Yellow
- Invade the enemy castle
- Steal diamonds
- Return them to your own castle

First team to complete the objective wins.

<table border="1" cellspacing="0">
 <tr>
   <th>Red Castle</th>
   <th>Yellow Castle</th>
 </tr>
 <tr>
   <td>
    <img src="./images/castles/red_castle.png" width="250" alt="Red castle" />
   </td>
   <td>
    <img src="./images/castles/yellow_castle.png" width="250" alt="Yellow castle" />
   </td>
 </tr>
</table>

---

## Core Gameplay

Each team protects three diamonds.
To win, your team must steal the enemy diamonds and deliver them to your own base before they do the same.

<table border="1" cellspacing="0">
 <tr>
   <th>Red Diamond</th>
   <th>Yellow Diamond</th>
 </tr>
 <tr>
   <td>
    <img src="./images/diamonds/red_diamond.png" width="100" alt="Red diamond" />
   </td>
   <td>
    <img src="./images/diamonds/yellow_diamond.png" width="100" alt="Yellow diamond" />
   </td>
 </tr>
</table>

---

## Playable Classes

Each team has three class types:

1. Archer
2. Knight
3. Mage

Every class has different attack behavior and combat role.
Winning requires class coordination, positioning, and smart timing.

<table border="1" cellspacing="0">
 <tr>
   <th>Class</th>
   <th>Red Team</th>
   <th>Yellow Team</th>
 </tr>
 <tr>
   <td>Archer</td>
   <td>
    <img src="./images/characters/red_archer.png" width="100" alt="Red archer" />
   </td>
   <td>
    <img src="./images/characters/yellow_archer.png" width="100" alt="Yellow archer" />
   </td>
 </tr>
 <tr>
   <td>Knight</td>
   <td>
    <img src="./images/characters/red_knight.png" width="100" alt="Red knight" />
   </td>
   <td>
    <img src="./images/characters/yellow_knight.png" width="100" alt="Yellow knight" />
   </td>
 </tr>
 <tr>
   <td>Mage</td>
   <td>
    <img src="./images/characters/red_mage.png" width="100" alt="Red mage" />
   </td>
   <td>
    <img src="./images/characters/yellow_mage.png" width="100" alt="Yellow mage" />
   </td>
 </tr>
</table>

---

## Gallery

<table border="1" cellspacing="0">
 <tr>
  <td>
   <img src="./images/gameplay/gameplay_1.png" width="350" alt="Gameplay 1" />
  </td>
  <td>
   <img src="./images/gameplay/gameplay_2.png" width="350" alt="Gameplay 2" />
  </td>
  <td>
   <img src="./images/gameplay/gameplay_3.png" width="350" alt="Gameplay 3" />
  </td>
 </tr>
 <tr>
  <td>
   <img src="./images/gameplay/gameplay_4.png" width="350" alt="Gameplay 4" />
  </td>
  <td>
   <img src="./images/gameplay/gameplay_5.png" width="350" alt="Gameplay 5" />
  </td>
  <td>
   <img src="./images/gameplay/gameplay_6.png" width="350" alt="Gameplay 6" />
  </td>
 </tr>
 <tr>
  <td>
   <img src="./images/gameplay/gameplay_7.png" width="350" alt="Gameplay 7" />
  </td>
 </tr>
</table>

---

## Installation and Setup

### Prerequisites

- Windows 10 or newer
- Visual Studio 2022 (recommended)
- .NET Framework 4.8 Developer Pack
- .NET SDK 3.1 (for server project)
- MonoGame 3.8+ framework and tools (DesktopGL / Content Builder)

### Install MonoGame (Important)

The client is a MonoGame DesktopGL project and needs MonoGame build targets and content tools installed.

1. Install MonoGame for Visual Studio 2022 (3.8 or newer).
2. Restart Visual Studio after installation.
3. From this repository, restore local MonoGame CLI tools:

```bash
cd CastleBridge.Client
dotnet tool restore
cd ..
```

4. Verify MonoGame tool installation:

```bash
dotnet tool list --local
```

You should see dotnet-mgcb in the local tools list.

If you get build errors related to missing MonoGame imports/targets, reinstall MonoGame and ensure Visual Studio was closed during installation.

### Clone Repository

```bash
git clone https://github.com/idanbachar/castle-bridge.git
cd castle-bridge
```

### Restore Dependencies

From the repository root:

```bash
dotnet restore CastleBridge.sln
```

---

## Run the Game

### 1) Start the server

```bash
dotnet run --project CastleBridge.Server/CastleBridge.Server.csproj
```

Default server endpoint is localhost on port 4441.

### 2) Start the client

Open [CastleBridge.sln](CastleBridge.sln) in Visual Studio and run the CastleBridge.Client project.

Notes:

- The client project is a classic .NET Framework MonoGame project.
- Visual Studio is the most reliable way to run it.

---

## Project Structure

- CastleBridge.Client: Game client, rendering, screens, HUD, characters, map logic
- CastleBridge.Server: Multiplayer server and networking loop
- CastleBridge.OnlineLibraries: Shared packets and networking data contracts
