const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
const dotenv = require('dotenv');
const Score = require('./models/Score');

dotenv.config();

const app = express();
app.use(cors());
app.use(express.json());

mongoose.connect(process.env.MONGO_URI)
  .then(() => console.log('✅ Connected to MongoDB Atlas'))
  .catch(err => console.error('❌ MongoDB connection error:', err));

// POST /submit-score
app.post('/submit-score', async (req, res) => {
  try {
    const { name, wallet, score } = req.body;

    if (!name || !wallet || typeof score !== 'number') {
      return res.status(400).json({ message: 'Invalid request data' });
    }

    const existing = await Score.findOne({ wallet });

    // 🧠 Update only if score is higher
    if (existing) {
      if (score > existing.score) {
        existing.score = score;
        existing.name = name; // Optional: allow name change
        await existing.save();
        return res.status(200).json({ message: 'Score updated (new high score)' });
      } else {
        return res.status(200).json({ message: 'Score not updated (not higher)' });
      }
    }

    // Create new user
    await Score.create({ name, wallet, score });
    res.status(201).json({ message: 'New score added' });

  } catch (err) {
    if (err.code === 11000) {
      return res.status(409).json({ message: 'Duplicate name or wallet' });
    }
    res.status(500).json({ message: 'Server error', error: err.message });
  }
});

// GET /leaderboard
app.get('/leaderboard', async (req, res) => {
  try {
    const topScores = await Score.find().sort({ score: -1 }).limit(10);
    res.json(topScores);
  } catch (err) {
    res.status(500).json({ message: 'Error fetching leaderboard', error: err.message });
  }
});

const PORT = process.env.PORT || 5000;
app.listen(PORT, () => {
  console.log(`🚀 Server running at http://localhost:${PORT}`);
});
