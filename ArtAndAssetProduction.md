# 10. Art & Asset Production

## 10.1. Art Direction
The visual style follows a **2D Pixel Art** aesthetic, prioritizing clarity and retro appeal.
*   **Pillars:**
    *   **Readability:** Characters and projectiles must clearly stand out against the background. High contrast is essential for the "Bullet Hell" gameplay.
    *   **Consistency:** All assets must share a consistent pixel resolution and color palette to maintain visual cohesion.
    *   **Feedback:** Visual elements should immediately communicate gameplay states (e.g., Open vs. Closed doors, Damage flashes).

## 10.2. Pipeline & Tools
*   **Engine:** Unity 2022+ (2D Core).
*   **Authoring Tools:**
    *   **Sprites/Animation:** Aseprite / Photoshop (or equivalent pixel art tool).
    *   **UI:** Figma (for layout prototyping) -> Unity Canvas.
*   **Import Settings:**
    *   **Filter Mode:** Point (No Filter) to preserve crisp pixel edges.
    *   **Compression:** RGB 24 bit (True Color) / None for pixel art.
    *   **Pixels Per Unit (PPU):** Consistent value (e.g., 16 or 32) across all assets to ensure uniform scale.

## 10.3. Characters (2D)
*   **Format:** Sprite Sheets.
*   **Orientation:** Top-down perspective.
*   **Requirements:**
    *   **Pivot:** Bottom-Center (at feet) for correct sorting and positioning.
    *   **Silhouette:** Distinct shapes for Player vs. Enemies to allow instant identification.

## 10.4. Environments & World Art
*   **Technology:** Unity Tilemap System.
*   **Architecture:**
    *   **Grid:** Rectangular grid layout.
    *   **Layers:**
        *   **Floor:** Background tiles (Walkable).
        *   **Walls:** Collidable tiles (Obstacles).
        *   **Decoration:** Non-collidable overlay tiles (Cracks, moss).
*   **Assets:**
    *   **Tile Palette:** Reusable tile sets for modular dungeon generation.
    *   **Doors:** Interactive `TileBase` assets managed by script (`Door.cs`).

## 10.5. Props & Interactables
*   **Projectiles:**
    *   **Arrow:** Simple, directional sprite that rotates based on velocity. Must have a collider triggers.
*   **Interactables:**
    *   **Doors:** Logic-driven sprites with two visual states (Open/Closed).

## 10.6. Animation
*   **System:** Unity Animator Controller.
*   **States:**
    *   **Idle:** Default loop.
    *   **Move:** Walk cycle triggered by velocity > 0.
    *   **Attack:** (Optional) Triggered by shooting action.
*   **Frame Rate:** 12-24 FPS for retro feel.

## 10.7. VFX & Particles
*   **Approach:** Sprite-based effects.
*   **Key Effects:**
    *   **Damage Flash:** `SpriteRenderer` color modulation (Red tint) upon taking damage.
    *   **Death:** Enemy removal (instantly destroyed or plays death frame).

## 10.10. 2D Art & Illustration
*   **UI Art:**
    *   **HUD:** Health bars (Foreground/Background fill images), XP Bars.
    *   **Menus:** Panel backgrounds, Buttons (Normal/Hover/Pressed states).
    *   **Iconography:** Simple, legible icons for stats (Health Heart, Speed Boot, Damage Sword).

## 10.12. Integration & QA
*   **Sorting Layers:**
    1.  **Background:** Floor tiles.
    2.  **Midground:** Walls, Props.
    3.  **Entities:** Player, Enemies (Dynamic sorting by Y-axis).
    4.  **Foreground:** Projectiles, VFX.
    5.  **UI:** Canvas Overlay.
*   **Collision:** All environment assets must have `TilemapCollider2D` or `BoxCollider2D` configured to the Physics Layer Matrix.
