# EndlessRacer2D# 🚗 Endless Racer 2D

**Endless Racer 2D** is a pseudo-3D endless runner game built in **Unity (2022.3.61f1)** targeting **WebGL**. Players race down a three-lane highway, dodging hazards and collecting bananas for bonus points, with integrated leaderboards and Web3 wallet input.

### 🎾 Play the Game:

🖐 **[Play Endless Racer 2D (WebGL Build)](https://endless-racer2-d-web-gl.vercel.app/)**

**Note:**  
GitHub README links open in the **same tab by default**.  
If you want to open the link in a **new tab**, use:

- **Ctrl + Click** (Windows/Linux)  
- **Cmd + Click** (Mac)  
- Or **Middle-click** the link

---

## 📦 Repository

**GitHub Repo (Source Code):**
[https://github.com/SahariKrithik/EndlessRacer2D](https://github.com/SahariKrithik/EndlessRacer2D)

*Note: The WebGL build repository is private and not required for local development.*

---

## 🚀 Features

* **Pseudo-3D Endless Racing Mechanics**
* **Speed Boost Visuals**:

  * Radial blur (UI overlay)
  * Speed lines (particles)
  * Dynamic FOV scaling
* **Leaderboard System** (backend-integrated via MongoDB Atlas)
* **Wallet-Based Score Tracking** (manual wallet input supported)
* **Optimized for WebGL** (no post-processing, minimal overdraw)
* **Cross-Platform:** Desktop browser play (mobile is blocked)

---

## 🛠️ Tech Stack

| Tech                         | Purpose                           |
| ---------------------------- | --------------------------------- |
| **Unity 2022.3.61f1**        | Game development                  |
| **Built-in Render Pipeline** | Performance-focused rendering     |
| **Vercel**                   | WebGL build hosting               |
| **Render.com**               | Backend API hosting               |
| **MongoDB Atlas**            | Leaderboard & player data storage |

---

## 💂️ Project Structure

| Folder                   | Purpose                                         |
| ------------------------ | ----------------------------------------------- |
| `Assets/Scripts/Backend` | Handles HTTP calls to backend                   |
| `Assets/Scripts/UI`      | UI management (leaderboard, score display)      |
| `Assets/Scripts/Game`    | Core game logic                                 |
| `Assets/StreamingAssets` | Leaderboard assets (if any)                     |
| `WebGLTemplates`         | Custom WebGL template (aspect ratio, loader UI) |

---

## ⚙️ How to Run Locally

1. **Clone the Repo**

```bash
git clone https://github.com/SahariKrithik/EndlessRacer2D.git
```

2. **Open in Unity**

* Use **Unity 2022.3.61f1** (Built-in Render Pipeline)

3. **Build for WebGL**

* Go to **File → Build Settings → WebGL → Build & Run**

---

## 🌐 Backend API

The backend handles:

* **Score Submission** (`/submit-score`)
* **Leaderboard Fetch** (`/leaderboard`)
* **User High Score Fetch** (`/user-high-score?wallet=...`)

Backend is deployed on **Render.com**.

---

## 📝 Known Issues

* No MetaMask integration (manual wallet input is used)
* WebGL build may vary slightly between browsers (Chrome recommended)

---

## 👍 License

This project is under **MIT License**
Feel free to use, modify, or contribute!
