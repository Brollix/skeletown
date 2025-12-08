# 8. Gameplay Design

## 8.1. Core Gameplay Loop

### 8.1.1. Overview
The core gameplay loop is a cycle of **Combat -> Upgrade -> Advancement**. The player enters a floor, eliminates all hostile entities to trigger the door unlock event, and proceeds to the next floor. This cycle repeats with increasing difficulty until the final Boss floor.

### 8.1.2. Primary Actions
*   **Movement:** Continuous navigation on a 2D plane to evade enemy contact and projectile attacks.
*   **Combat:** Ranged attacks using a projectile-based weapon system.
*   **Resource Collection:** Accumulating Experience Points (XP) dropped by defeated enemies to fuel the progression system.

### 8.1.3. Objectives and Rewards
*   **Floor Objective:** Reduce the `EnemiesRemaining` counter to 0.
*   **Reward:**
    *   **Immediate:** Access to the next floor (Door state changes to Open).
    *   **Cumulative:** XP gain leading to Level Ups and Skill Points.

### 8.1.4. Feedback Loop
*   **Positive Reinforcement:**
    *   **Audio:** Distinct "Death" sound plays upon enemy elimination.
    *   **Visual:** XP Bar fills dynamically; Door sprite changes from "Closed" to "Open" upon floor completion.
*   **Negative Reinforcement:**
    *   **Damage:** Player sprite flashes (Invulnerability state) and Health Bar decreases.
    *   **Audio:** "Hurt" sound plays to signal error/failure.

## 8.2. Controls

### 8.2.1. Input Devices
*   **Primary:** Keyboard & Mouse.

### 8.2.2. Control Schemes
*   **Movement (WASD):**
    *   **W:** Move Up (+Y axis).
    *   **A:** Move Left (-X axis).
    *   **S:** Move Down (-Y axis).
    *   **D:** Move Right (+X axis).
*   **Combat:**
    *   **Mouse Position:** Determines the rotation of the weapon/aim direction.
    *   **Left Mouse Button:** Instantiates a projectile in the aimed direction (subject to 0.3s cooldown).
*   **System:**
    *   **P:** Toggles Debug/Diagnostic overlay.
    *   **Escape:** Toggles Pause Menu.

## 8.3. Progression Systems

### 8.3.1. Experience and Leveling
*   **XP Acquisition:** Enemies grant fixed XP values upon death (e.g., 60 XP).
*   **Leveling Formula:** The XP required for the next level follows an exponential curve:
    *   `XP_Required = 100 * (1.2 ^ (CurrentLevel - 1))`
    *   *Note: Capped at 250 XP per level to prevent excessive grinding.*

### 8.3.2. Skill Trees and Abilities
Upon leveling up, the player earns **1 Skill Point**, which can be exchanged for one of the following permanent stat upgrades:
*   **Health Upgrade:** Adds **+2.0** to Max Health (Base: 10.0).
*   **Damage Upgrade:** Adds **+0.5** to Projectile Damage (Base: 1.0).
*   **Speed Upgrade:** Adds **+0.5** to Movement Speed (Base: 5.0).

### 8.3.3. Unlockables
*   **Floor Progression:** Clearing Floor `N` unlocks Floor `N+1`.
*   **Victory:** Clearing the final Boss floor unlocks the Victory Screen.

### 8.3.6. Progression Pacing
*   **Scaling:** Enemy difficulty scales linearly by floor, while player power scales step-wise via upgrades. The **20% enemy stat increase per floor** forces the player to prioritize high-value upgrades (Damage/Health) to keep pace.

## 8.4. Difficulty and Balance

### 8.4.2. Enemy and Challenge Scaling
Enemies do not have static stats. Their attributes are calculated at spawn based on the current `FloorNumber`:
*   **Scaling Formula:** `Multiplier = 1.0 + ((FloorNumber - 1) * 0.20)`
*   **Affected Stats:**
    *   **Health:** `BaseHealth * Multiplier`
    *   **Damage:** `BaseDamage * Multiplier`
    *   **Speed:** `BaseSpeed * Multiplier`
*   *Example:* On Floor 6, enemies have **200% (2x)** their base stats.

### 8.4.3. Resource Distribution
*   **Player Health:** Starts at **10.0**. No passive regeneration.
*   **Invulnerability:** Upon taking damage, the player enters an invulnerable state for **1.0 second** to prevent "stunlock" deaths.

## 8.5. Feedback Systems

### 8.5.1. Visual Feedback
*   **HUD:**
    *   **Health Bar:** Red fill amount = `CurrentHealth / MaxHealth`.
    *   **XP Bar:** Blue/Green fill amount = `CurrentXP / XPForNextLevel`.
*   **World:**
    *   **Projectiles:** Visual arrow sprites travel at constant velocity.
    *   **Damage Numbers:** (Debug) Console logs verify damage calculations.

### 8.5.2. Audio Feedback
*   **SFX:**
    *   `ShootSound`: Plays on successful fire (not on cooldown).
    *   `HitSound`: Plays when Player `CurrentHealth` decreases.
    *   `DeathSound`: Plays when Player `CurrentHealth` <= 0.
    *   `Hover/Click`: UI interaction feedback.

## 8.6. Replayability
*   **Procedural Challenge:** While the layout is static, the **Infinite Scaling** allows for "High Score" runs where the player attempts to survive as many floors as possible against exponentially stronger enemies.
*   **Build Diversity:** Players can experiment with different upgrade distributions (e.g., "Speedrunner" build vs. "Tank" build).
