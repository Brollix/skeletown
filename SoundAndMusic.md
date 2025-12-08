# 11. Sound & Music

## 11.1. Audio Direction & Vision
The audio direction focuses on **clarity and feedback**. Given the chaotic nature of the "Bullet Hell" genre, sound effects serve as critical gameplay indicators rather than purely atmospheric elements.
*   **Pillars:**
    *   **Feedback First:** Every gameplay action (shooting, hitting, taking damage) has a distinct, punchy sound to confirm the event.
    *   **Non-Spatial:** All sounds are 2D (stereo) to ensure they are clearly audible regardless of the player's position on the screen.
    *   **Dynamic Pitching:** Repetitive sounds (like shooting) use random pitch variation to prevent auditory fatigue.

## 11.2. Music Exploration
*   **Direction:** The music underscores the tension of the dungeon environment.
*   **Implementation:**
    *   **Dungeon Theme:** A looping track that plays continuously during gameplay to maintain momentum.
    *   **Silence:** The Main Menu is intentionally silent (or uses a minimal drone) to contrast with the high-energy gameplay loop.
*   **Logic:** Music is managed by the `SceneLoader`, which triggers `AudioManager.Instance.PlayMusic()` upon entering the Dungeon and `StopMusic()` when returning to the Menu.

## 11.3. Sound Effect Exploration
*   **Style:** Retro/Arcade-inspired synthesized sounds.
*   **Key SFX:**
    *   **Shoot:** A sharp, short "thwip" or laser sound. Uses a pitch variance of **0.9 to 1.1** to create variety in rapid-fire scenarios.
    *   **Hit:** A "crunch" or impact sound to confirm damage dealing.
    *   **Hurt:** A distinct, alarming sound to signal player damage.
    *   **UI:** High-frequency "clicks" for button interactions to provide tactile responsiveness.

## 11.7. Audio Implementation & Middleware
*   **Engine:** Unity Audio System (Native).
*   **Architecture:**
    *   **AudioManager Singleton:** A central manager that persists across scenes (`DontDestroyOnLoad`).
    *   **AudioMixer:** A hierarchical mixing graph with three groups:
        1.  **Master:** Global volume control.
        2.  **Music:** Dedicated channel for BGM.
        3.  **SFX:** Dedicated channel for gameplay sounds.
*   **Pooling:** The `PlaySFX` method instantiates temporary `AudioSource` objects for each sound event and automatically destroys them after playback, allowing for multiple overlapping sounds without cutting each other off.

## 11.8. Audio Asset Specifications
*   **Format:**
    *   **Music:** OGG Vorbis (Compressed for streaming/memory efficiency).
    *   **SFX:** WAV (Uncompressed/PCM for low-latency playback).
*   **Sample Rate:** 44.1 kHz.
*   **Bit Depth:** 16-bit.

## 11.9. Asset List & References

| Category | Asset Name | Description | Trigger |
| :--- | :--- | :--- | :--- |
| **Music** | `DungeonTheme` | High-tempo combat loop | Scene Load (Dungeon) |
| **SFX** | `ShootSound` | Projectile fire sound | Left Mouse Click (Player) |
| **SFX** | `HitSound` | Impact crunch | Enemy takes damage |
| **SFX** | `DeathSound` | Enemy elimination | Enemy HP <= 0 |
| **SFX** | `PlayerHurt` | Damage warning | Player HP decreases |
| **SFX** | `UIClick` | Menu interaction | Button Press |
| **SFX** | `UIHover` | Menu navigation | Mouse Enter Button |

## 11.10. Mixing & Mastering Guidelines
*   **Hierarchy:**
    *   **SFX (Highest Priority):** Must cut through the mix to provide combat feedback.
    *   **Music (Background):** Mixed -3dB to -6dB lower than SFX to prevent masking gameplay cues.
*   **Pitch Variation:** All repetitive gameplay SFX utilize a random pitch range of **+/- 10% (0.9 - 1.1)** to reduce "machine-gun" repetition artifacts.
