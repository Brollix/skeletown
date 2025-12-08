# 9. Game Mechanics

## 9.1. Core Mechanics

### 9.1.1. Primary Gameplay Elements
*   **Movement:**
    *   **Type:** Uniform Rectilinear Motion (MRU) on a 2D plane.
    *   **Speed:** Constant base speed of **5.0 units/second**.
    *   **Modifier:** Speed can be increased by **+0.5 units/second** per Speed Upgrade purchased.
*   **Ranged Combat:**
    *   **Projectile:** Arrows are instantiated at the player's position and travel in the direction of the mouse cursor.
    *   **Fire Rate:** Limited by a **0.3-second cooldown** between shots.
    *   **Damage:** Base damage is **1.0**, increasing by **+0.5** per Damage Upgrade.
*   **Room Clearing:**
    *   **Condition:** The `EnemiesRemaining` counter must reach **0**.
    *   **Trigger:** Upon condition met, the `Door` object executes `SetOpen(true)`, disabling its collider and changing its sprite.

### 9.1.2. Game Rules and Systems
*   **Infinite Scaling Rule:**
    *   Every time the player advances a floor, the global difficulty multiplier increases.
    *   **Formula:** `Difficulty = 1.0 + (FloorIndex * 0.2)`
    *   **Effect:** A floor 5 enemy has **2.0x (200%)** the stats of a floor 0 enemy.
*   **Health & Failure:**
    *   **Base HP:** **10.0 Hit Points**.
    *   **Invulnerability:** Upon taking damage, the player becomes immune to all damage for **1.0 second**.
    *   **Game Over:** Triggered immediately when `CurrentHealth <= 0`.
*   **Victory Condition:**
    *   Defeating the entity flagged `isBoss = true` on the final floor triggers the `VictoryUI`.

### 9.1.3. Progression Mechanics
*   **Experience (XP):**
    *   **Base Requirement:** Level 1 requires **100 XP**.
    *   **Scaling:** Requirement increases by **20% (1.2x)** per level.
    *   **Cap:** The requirement is hard-capped at **250 XP** to prevent the curve from becoming insurmountable.
*   **Skill Points:**
    *   **Acquisition:** +1 Point per Level Up.
    *   **Economy:** Points are the sole currency for the Upgrade Manager.

## 9.3. Interaction Mechanics

### 9.3.1. Player-Environment Interaction
*   **Doors:**
    *   **State 0 (Locked):** `BoxCollider2D` is Enabled (Impassable). Sprite is "Closed Gate".
    *   **State 1 (Unlocked):** `BoxCollider2D` is Disabled (Passable). Sprite is "Open Void".
*   **Walls:**
    *   **Properties:** Static `TilemapCollider2D` objects.
    *   **Interaction:** Blocks both `Player` movement (Rigidbody collision) and `Projectile` trajectory (Trigger enter).

### 9.3.4. Feedback and Cues
*   **Visual Cues:**
    *   **Damage:** The `SpriteRenderer` color is not modified, but the Health Bar UI updates instantly.
    *   **Projectiles:** Arrows are destroyed upon impact with any collider (Wall or Enemy).
*   **Audio Cues:**
    *   **Spatial:** Sounds are 2D (non-spatial) for clarity.
    *   **Priority:** "Player Death" and "Level Up" sounds have high priority in the mix.

## 9.4. Economy and Currencies

### 9.4.1. In-Game Currencies
*   **Skill Points:**
    *   **Type:** Integer value.
    *   **Storage:** Persists in `PlayerExperience` singleton.
    *   **Loss:** Reset to 0 upon Game Over (Roguelite run reset).

### 9.4.2. Resource Management
*   **Health:**
    *   **Regeneration:** **0.0 HP/sec** (No passive regen).
    *   **Restoration:** Currently no healing items; health is a strictly depleting resource per run.
*   **Cooldowns:**
    *   **Shoot Timer:** A float value `cooldownTimer` decrements by `Time.deltaTime` every frame. Firing is disabled while `cooldownTimer > 0`.

### 9.4.3. Trading and Commerce
*   **Upgrade Menu:**
    *   **Transaction:** 1 Skill Point <-> 1 Stat Upgrade.
    *   **Availability:** Accessible anytime via the Pause Menu (P key / Escape).
